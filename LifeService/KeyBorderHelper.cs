using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LifeService
{
    /// <summary>
    /// 键盘调用
    /// </summary>
    public static class KeyBorderHelper
    {
        private const Int32 WM_SYSCOMMAND = 274;
        private const UInt32 SC_CLOSE = 61536;
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegisterWindowMessage(string lpString);

        /// <summary>
        /// 显示屏幕键盘
        /// </summary>
        /// <returns></returns>
        public static int ShowInputPanel()
        {
            try
            {
                dynamic file = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";
                if (!System.IO.File.Exists(file))
                    return -1;
                Process.Start(file);
                return 0;
            }
            catch (Exception)
            {
                return 255;
            }
        }

        /// <summary>
        /// 隐藏屏幕键盘
        /// </summary>
        public static void HideInputPanel()
        {
            IntPtr TouchhWnd = new IntPtr(0);
            TouchhWnd = FindWindow("IPTip_Main_Window", null);
            if (TouchhWnd == IntPtr.Zero)
                return;
            PostMessage(TouchhWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
        }
    }
}
