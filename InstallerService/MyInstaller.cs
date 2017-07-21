using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace InstallerService
{
    [RunInstaller(true)]
    public partial class MyInstaller : System.Configuration.Install.Installer
    {
        string AppName = "DesktopWeather";
        string AppFile = AppDomain.CurrentDomain.BaseDirectory + "DesktopWeather.exe";

        public MyInstaller()
        {
            InitializeComponent();

            AfterInstall += MyInstaller_AfterInstall;

            AfterUninstall += MyInstaller_AfterUninstall;
        }

        private void MyInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            string regAll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            bool isSuccess = RegisterTool.DeleteValue(regAll, AppName);
            string regCurrent = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
            isSuccess = RegisterTool.DeleteValue(regCurrent, AppName);
        }

        private void MyInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            string regAll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            bool isSuccess = RegisterTool.SetValue(regAll, AppName, AppFile);
            string regCurrent = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
            isSuccess = RegisterTool.SetValue(regCurrent, AppName, AppFile);

            System.Diagnostics.ProcessStartInfo psiConfig = new System.Diagnostics.ProcessStartInfo(this.Context.Parameters["targetdir"] + "\\DesktopWeather.exe");

            System.Diagnostics.Process pConfig = System.Diagnostics.Process.Start(psiConfig);
        }
    }
}
