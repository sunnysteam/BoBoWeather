# DesktopWeather
## 使用Wpf开发的桌面天气插件

### 1、功能：
1. 能够获取当天的天气和未来三天的天气<br>
2. 支持城市查询<br>
3. 闹钟功能<br>
4. 备忘提醒功能<br>
### 2、关键代码:
* 天气读取代码<br>
```C#
        /// <summary>
        /// 调用天气
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WeatherDataModel WeatherData(string url)
        {
            WeatherDataModel model = new WeatherDataModel();
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.ContentType = "multipart/form-data";
                req.Accept = "*/*";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                req.Timeout = 30000; //30秒连接不成功就中断 
                req.Method = "GET";

                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string result = sr.ReadToEnd();
                    model = JsonConvert.DeserializeObject<WeatherDataModel>(result);
                }
            }
            catch (WebException ex)
            {
                model = null;
            }
            return model;
        }
```
* 嵌入桌面代码<br>
```C#
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
```
* 开机自启动代码<br>
```C#
        ////设置开机自动启动
        string regAll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        bool isSuccess = RegisterTool.SetValue(regAll, AppName, AppFile);
        string regCurrent = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
        isSuccess = RegisterTool.SetValue(regCurrent, AppName, AppFile);

        /// <summary>
        /// 添加注册表值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string key, string name, string value)
        {
            try
            {
                using (RegistryKey RKey = Create(key))
                {
                    RKey.SetValue(name, value);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除注册表值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool DeleteValue(string key, string name)
        {
            try
            {
                using (RegistryKey RKey = Open(key, true))
                {
                    if (RKey != null)
                        RKey.DeleteValue(name);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
```
### 3、程序包下载：
    你可以直接下载程序包使用：<br>
    [安装包下载](http://pan.baidu.com/s/1bpD4tp9)<br>
    提取码 zkp2
