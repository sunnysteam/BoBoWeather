using LifeService;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using TaskScheduler;

namespace DesktopWeather
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 系统API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);
        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, // handle to destination window 
        uint Msg, // message 
        int wParam, // first message parameter 
        int lParam // second message parameter 
        );
        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);
        #endregion


        string AppName = "DesktopWeather";
        static string AppFile = AppDomain.CurrentDomain.BaseDirectory + "DesktopWeather.exe";
        WindowState lastWindowState;
        public MainWindow()
        {
            InitializeComponent();

            //默认设置
            //关掉已运行的进程实例
            Process ps = GetRunningInstance();
            if (ps != null) ps.Kill();

            ////设置开机自动启动
            string regAll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            bool isSuccess = RegisterTool.SetValue(regAll, AppName, AppFile);
            string regCurrent = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
            isSuccess = RegisterTool.SetValue(regCurrent, AppName, AppFile);

            //string regAll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            //bool isSuccess = RegisterTool.DeleteValue(regAll, AppName);
            //string regCurrent = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
            //isSuccess = RegisterTool.DeleteValue(regCurrent, AppName);
        }

        //获取已运行的进程实例
        public static Process GetRunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess(); //获取当前进程 
            //获取当前运行程序完全限定名 
            string currentFileName = currentProcess.MainModule.FileName;
            //获取进程名为ProcessName的Process数组。 
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            //遍历有相同进程名称正在运行的进程 
            foreach (Process process in processes)
            {
                if (process.MainModule.FileName == currentFileName)
                {
                    if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                        return process;//返回已运行的进程实例 
                }
            }
            return null;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            lastWindowState = WindowState;
        }

        private void maingrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                maingrid.Children.Add(LifeServicePlugin.Instance.LifeServiceView);
                Left = SystemParameters.WorkArea.Width - Width;
                Top = 50;

                IntPtr desktopHwnd = GetDesktopPtr();
                IntPtr result = SetParent(new WindowInteropHelper(this).Handle, desktopHwnd);

                InstallFonts(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
            
        }

        /// <summary>
        /// 将程序嵌入桌面
        /// </summary>
        /// <returns></returns>
        private IntPtr GetDesktopPtr()//寻找桌面的句柄
        {
            // 情况一
            IntPtr hwndWorkerW = IntPtr.Zero;
            IntPtr hShellDefView = IntPtr.Zero;
            IntPtr hwndDesktop = IntPtr.Zero;
            IntPtr hProgMan = FindWindow("Progman", "Program Manager");
            if (hProgMan != IntPtr.Zero)
            {
                hShellDefView = FindWindowEx(hProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (hShellDefView != IntPtr.Zero)
                {
                    hwndDesktop = FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
                }
            }
            if (hwndDesktop != IntPtr.Zero) return hwndDesktop;

            // 情况二
            //必须存在桌面窗口层次
            while (hwndDesktop == IntPtr.Zero)
            {
                //获得WorkerW类的窗口
                hwndWorkerW = FindWindowEx(IntPtr.Zero, hwndWorkerW, "WorkerW", null);
                if (hwndWorkerW == IntPtr.Zero) break;
                hShellDefView = FindWindowEx(hwndWorkerW, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (hShellDefView == IntPtr.Zero) continue;
                hwndDesktop = FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
            }
            return hwndDesktop;
        }

        /// <summary>
        /// 安装单个字体
        /// </summary>
        /// <param name="f"></param>
        /// <param name="Dir"></param>
        private void SetFonts(FileInfo f, DirectoryInfo Dir)
        {
            string WinFontDir = Environment.GetEnvironmentVariable("windir") + "\\fonts\\";
            string FontFileName = f.Name;
            string FontName = f.Name.Replace(f.Extension,"");
            int Ret;
            int Res;
            string FontPath;
            const int WM_FONTCHANGE = 0x001D;
            const int HWND_BROADCAST = 0xffff;
            FontPath = WinFontDir + FontFileName;
            if (!File.Exists(FontPath))
            {
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\" + f.Name, FontPath);
                Ret = AddFontResource(FontPath);

                Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                Ret = WriteProfileString("fonts", FontName + "(TrueType)", FontFileName);
            }

        }

        /// <summary>
        /// 安装字体
        /// </summary>
        /// <param name="dirPath"></param>
        private void InstallFonts(string dirPath)
        {
            //在指定目录及子目录下查找文件,在listBox1中列出子目录及文件
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            try
            {
                foreach (DirectoryInfo d in dir.GetDirectories())//查找子目录
                {
                    InstallFonts(dir + d.ToString() + "\\");
                }
                int item = 0;
                double items = dir.GetFiles().Length;
                foreach (FileInfo f in dir.GetFiles()) //查找文件
                {
                    SetFonts(f, dir);
                    item++;
                }
            }
            catch (Exception e)
            {
            }
        }

        private void maingrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                (sender as Window).DragMove();
            }
            catch (Exception ex)
            {
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            LifeServicePlugin.Instance.Dispose();
            Close();
        }
    }
}
