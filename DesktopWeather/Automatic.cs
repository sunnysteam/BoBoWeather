using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopWeather
{
    public class Automatic
    {
        //判断是否已经存在此键值,此处可以在Form_Load中来使用。
        //如果存在，菜单[开机自动运行]前面可以打上对钩
        //如果不存在，则不操作
        public bool IsExistKey(string keyName)
        {
            bool _exist = false;

            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey runs = hklm.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (runs == null)
            {
                RegistryKey key2 = hklm.CreateSubKey("SOFTWARE");
                RegistryKey key3 = key2.CreateSubKey("Microsoft");
                RegistryKey key4 = key3.CreateSubKey("Windows");
                RegistryKey key5 = key4.CreateSubKey("CurrentVersion");
                RegistryKey key6 = key5.CreateSubKey("Run");
                runs = key6;
            }
            //注意此处用的是GetValueNames()
            string[] runsName = runs.GetValueNames();
            foreach (string strName in runsName)
            {
                if (strName.ToUpper() == keyName.ToUpper())
                {
                    _exist = true;
                    return _exist;
                }
            }

            return _exist;
        }

        public bool WriteKey(string keyName, string keyValue)
        {
            RegistryKey hklm = Registry.LocalMachine;

            //定义hklm指向注册表的LocalMachine,其中Software/Microsoft/Windows/CurrentVersion/Run就是关系到系统中随系统启动而启动的程序，通称启动项 
            RegistryKey run = hklm.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            try
            {
                //将我们的程序加进去
                run.SetValue(keyName, keyValue);

                //注意，一定要关闭，注册表应用。
                hklm.Close();

                return true;
            }
            catch //这是捕获异常的 
            {
                return false;
            }
        }

        //删除键值
        public void DeleteKey(string keyName)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey runs = hklm.OpenSubKey(@"Software/Microsoft/Windows/CurrentVersion/Run", true);

            try
            {
                //注意此处用的是GetValueNames()
                string[] runsName = runs.GetValueNames();
                foreach (string strName in runsName)
                {
                    if (strName.ToUpper() == keyName.ToUpper())
                        runs.DeleteValue(strName, false);
                }
            }
            catch { }
        }


    }
}
