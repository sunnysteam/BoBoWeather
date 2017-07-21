using System.IO;
using System.Net;
using System.Text;
using LifeService.Model.Weathermodel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System;

namespace LifeService.ViewModel.WeatherViewModel
{
    internal class WeatherManager
    {
        private static WeatherManager _instance;
        private static readonly object Padlock = new object();
        private static ReaderWriterLockSlim WriteLock = new ReaderWriterLockSlim();

        public static WeatherManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new WeatherManager();
                    }
                    return _instance;
                }
            }
        }

        private string City { get; set; }

        private WeatherDataModel WeatherNow { get; set; }

        private WeatherDataModel WeatherDaily { get; set; }

        /// <summary>
        /// 判定是否第一次启动或修改了城市
        /// 是：调用
        /// 否：直接取属性以减少调用开支
        /// </summary>
        /// <param name="url"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public WeatherDataModel GetWeatherDataNow(string url, string city)
        {
            if (WeatherNow == null)//是否第一次启动
            {
                WeatherNow = WeatherData(url);
            }
            else if(city != City)//是否修改了城市
            {
                WeatherNow = WeatherData(url);
            }
            return WeatherNow;
        }

        /// <summary>
        /// 判定是否第一次启动或修改了城市
        /// 是：调用
        /// 否：直接取属性以减少调用开支
        /// </summary>
        /// <param name="url"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public WeatherDataModel GetWeatherDataDaily(string url, string city)
        {
            if (WeatherDaily == null)//是否第一次启动
            {
                WeatherDaily = WeatherData(url);
            }
            else if (city != City)//是否修改了城市
            {
                WeatherDaily = WeatherData(url);
            }
            City = city;
            return WeatherDaily;
        }

        /// <summary>
        /// 调用心知天气
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

        /// <summary>
        /// 宿主软件调用3天的天气
        /// </summary>
        /// <returns></returns>
        public string[] ReturnDailyWeather()
        {
            if (WeatherDaily != null)
            {
                string today = string.Format("今天天气{0}，最高气温{1}度，最低气温{2}度", WeatherDaily.Results[0].Daily[0].Text_Day, WeatherDaily.Results[0].Daily[0].High, WeatherDaily.Results[0].Daily[0].Low);
                string tomorrow = string.Format("明天天气{0}，最高气温{1}度，最低气温{2}度", WeatherDaily.Results[0].Daily[1].Text_Day, WeatherDaily.Results[0].Daily[1].High, WeatherDaily.Results[0].Daily[1].Low);
                string thedayaftertomorrow = string.Format("后天天气{0}，最高气温{1}度，最低气温{2}度", WeatherDaily.Results[0].Daily[2].Text_Day, WeatherDaily.Results[0].Daily[2].High, WeatherDaily.Results[0].Daily[2].Low);
                string[] daily = new string[] { today, tomorrow, thedayaftertomorrow };
                return daily;
            }
            else
            {
                return null;
            }
        }

        public void WriteWeatherHistory(WeatherDataModel now, WeatherDataModel daily)
        {
            try
            {
                WriteLock.EnterWriteLock();
                FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WeatherHistory.txt"), FileMode.Create);
                fs.Close();
                StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WeatherHistory.txt"));
                string nowstr = JsonConvert.SerializeObject(now);
                string dailystr = JsonConvert.SerializeObject(daily);
                //开始写入
                sw.Write(nowstr + "##" + dailystr);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                
            }
            catch (System.Exception e)
            {
                LogWriter.Instance.Error(e);
            }
            finally
            {
                WriteLock.ExitWriteLock();
            }
        }

        public WeatherDataModel[] ReadWeatherHistory()
        {
            try
            {
                StreamReader sw = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WeatherHistory.txt"));
                string tempstr = sw.ReadToEnd();
                string[] reminders = Regex.Split(tempstr, "##", RegexOptions.IgnoreCase);
                WeatherDataModel now = JsonConvert.DeserializeObject<WeatherDataModel>(reminders[0]);
                WeatherDataModel daily = JsonConvert.DeserializeObject<WeatherDataModel>(reminders[1]);
                sw.Close();
                return new WeatherDataModel[] { now, daily };
            }
            catch (System.Exception e)
            {
                return null;
            }
            
        }
    }
}
