using System;
using GalaSoft.MvvmLight.Threading;
using LifeService.ViewModel;
using System.Windows.Controls;
using LifeService.ViewModel.ReminderViewModel;
using LifeService.ViewModel.AlarmClockViewModel;
using LifeService.ViewModel.WeatherViewModel;
using System.Windows;
using LifeService.Model.ReminderModel;
using System.Text.RegularExpressions;
using LifeService.Model;
using LifeService.View.ReminderView;
using LifeService.View.AlarmClockView;
using System.Threading.Tasks;

namespace LifeService
{
    /// <summary>
    /// 生活服务模块
    /// </summary>
    public class LifeServicePlugin
    {
        private static LifeServicePlugin _instance = null;
        private static readonly object Padlock = new object();

        /// <summary>
        /// 生活服务模块单例
        /// </summary>
        public static LifeServicePlugin Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new LifeServicePlugin();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 提醒语音播报
        /// </summary>
        public Action<string, bool> ReminderReporter;

        /// <summary>
        /// 关闭生活服务模块
        /// </summary>
        public Action CloseLifeServiceAction;

        private MainView _lifeServiceView;
        /// <summary>
        /// 生活服务模块属性
        /// </summary>
        public MainView LifeServiceView
        {
            get { return _lifeServiceView; }
            set { _lifeServiceView = value; }
        }

        private LifeServicePlugin()
        {
            DispatcherHelper.Initialize();
            _lifeServiceView = new MainView();
            _lifeServiceView.DataContext = new MainWindowViewModel();
            DataManager.Instance.StartConnect();//开启数据连接
            //实例化闹钟、提醒
            ReminderManager.Instance.ReminderReporter += ReminderFromReporter;
            MainAlarmClockManager.Instance.ToString();

        }

        /// <summary>
        /// 卸载生活服务模块
        /// </summary>
        public void Dispose()
        {
            try
            {
                DataManager.Instance.CloseConnect();
                ReminderManager.Instance.CloseReminder();
                MainAlarmClockManager.Instance.CloseAlarmClock();
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 获得天气信息
        /// </summary>
        /// <returns></returns>
        public string[] GetDailyWeather()
        {
            return WeatherManager.Instance.ReturnDailyWeather();
        }

        /// <summary>
        /// 设置远程提醒
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public void SetRemoteReminder(string str)
        {
            string[] reminders = Regex.Split(str, "::", RegexOptions.IgnoreCase);
            ReminderModel model = new ReminderModel();
            model.GuiD = Guid.NewGuid().ToString().Replace("-", "");
            model.ReminderTime = reminders[2] + " " + reminders[3];
            model.Content = reminders[1];
            model.Contact = "App提醒";
            model.Ring = "Faded.mp3";
            DataManager.Instance.InsertTables(model);
            ReminderManager.Instance.Reminders.Add(model);
        }

        /// <summary>
        /// 外部调用闹钟
        /// </summary>
        public void OpenAlarmClock()
        {
            MainAlarmClockView view = new MainAlarmClockView();
            view.DataContext = new MainAlarmClockViewModel();
            view?.Show();
        }

        /// <summary>
        /// 外部调用备忘提醒
        /// </summary>
        public void OpenMemoReminder()
        {
            ReminderView view = new ReminderView();
            view.DataContext = new ReminderViewModel("备忘提醒");
            view?.Show();
        }

        /// <summary>
        /// 外部调用远程提醒
        /// </summary>
        public void OpenRemoteReminder()
        {
            ReminderView view = new ReminderView();
            view.DataContext = new ReminderViewModel("远程提醒");
            view?.Show();
        }

        public void CloseLifeService()
        {
            CloseLifeServiceAction?.Invoke();
        }

        /// <summary>
        /// 提醒语音播报回调
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isReport"></param>
        private void ReminderFromReporter(string content, bool isReport)
        {
            ReminderReporter?.Invoke(content, isReport);
        }
    }
}
