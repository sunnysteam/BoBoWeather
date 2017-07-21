using GalaSoft.MvvmLight.Threading;
using LifeService.Model;
using LifeService.Model.ReminderModel;
using LifeService.View.ReminderView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace LifeService.ViewModel.ReminderViewModel
{
    internal class ReminderManager
    {
        private static ReminderManager _instance;
        private static readonly object Padlock = new object();
        private Timer _reportTimer = null;
        public static ReminderManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ReminderManager();
                    }
                    return _instance;
                }
            }
        }

        private ReminderManager()
        {
            FetchData();
            ReminderMonitor();
        }

        /// <summary>
        /// 提醒语音播报
        /// </summary>
        public Action<string, bool> ReminderReporter;


        private List<ReminderRingView> _remindersWindows = new List<ReminderRingView>();
        private List<ReminderModel> _reminders = new List<ReminderModel>();
        public List<ReminderModel> Reminders
        {
            get { return _reminders; }
            set { _reminders = value; }
        }

        /// <summary>
        /// 星期
        /// </summary>
        private string TodayWeek
        {
            get
            {
                string weekstr = DateTime.Now.DayOfWeek.ToString();
                switch (weekstr)
                {
                    case "Monday": weekstr = "周一"; break;
                    case "Tuesday": weekstr = "周二"; break;
                    case "Wednesday": weekstr = "周三"; break;
                    case "Thursday": weekstr = "周四"; break;
                    case "Friday": weekstr = "周五"; break;
                    case "Saturday": weekstr = "周六"; break;
                    case "Sunday": weekstr = "周日"; break;
                }
                return weekstr;
            }
        }

        /// <summary>
        /// 提醒监视
        /// </summary>
        private void ReminderMonitor()
        {
            _reportTimer = new Timer(new TimerCallback(callback =>
            {
                try
                {
                    foreach (var reminder in Reminders.ToArray())
                    {
                        if (string.IsNullOrEmpty(reminder.Contact))
                        {
                            TimeSpan date = DateTime.Now - (Convert.ToDateTime(reminder.ReminderTime));
                            if (date.TotalSeconds <= 10 && date.TotalSeconds >= 0)
                            {
                                ReminderRing(reminder);
                            }
                        }
                        else
                        {
                            //todo:远程提醒
                            if (reminder.ReminderTime.Contains("每天"))
                            {
                                string week = "周一 周二 周三 周四 周五 周六 周日";
                                if (week.Contains(TodayWeek))
                                {
                                    MatchTime(reminder);
                                }
                            }
                            else if (reminder.ReminderTime.Contains("工作日"))
                            {
                                string week = "周一 周二 周三 周四 周五";
                                if (week.Contains(TodayWeek))
                                {
                                    MatchTime(reminder);
                                }
                            }
                            else if (reminder.ReminderTime.Contains("周末"))
                            {
                                string week = "周六 周日";
                                if (week.Contains(TodayWeek))
                                {
                                    MatchTime(reminder);
                                }
                            }
                            else if (reminder.ReminderTime.Contains("只提醒一次"))
                            {
                                MatchTime(reminder);
                            }
                            else
                            {
                                if (reminder.ReminderTime.Contains(TodayWeek))
                                {
                                    MatchTime(reminder);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Instance.Error(ex);
                }
            }), null, 5, 10000); //一天执行一次=1000 * 60 * 60 * 24
        }

        private void MatchTime(ReminderModel reminder)
        {
            try
            {
                string[] times = reminder.ReminderTime.Split(' ');
                TimeSpan date;
                if (times[0].Equals("只提醒一次"))
                {
                    date = DateTime.Now - (Convert.ToDateTime(times[1] + " " + times[2]));
                }
                else
                {
                    date = DateTime.Now - (Convert.ToDateTime(times[times.Length - 1]));
                }
                if (date.TotalSeconds <= 10 && date.TotalSeconds >= 0)
                {
                    ReminderRing(reminder);
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        private void ReminderRing(ReminderModel reminder)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            ReminderRingView view = new ReminderRingView();
                            view.DataContext = new ReminderRingViewModel(reminder);
                            (view.DataContext as ReminderRingViewModel).ReminderReporter += ReminderFromReporter;
                            _remindersWindows.Add(view);
                            view.Show();
                        });
        }

        public void CloseReminder()
        {
            try
            {
                if (_remindersWindows != null && _remindersWindows.Count > 0)
                {
                    foreach (var item in _remindersWindows)
                    {
                        item.Close();
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        private void FetchData()
        {
            try
            {
                ReminderModel model = new ReminderModel();
                DataTable datas = DataManager.Instance.SelectTables(model);
                Reminders.Clear();
                foreach (DataRow data in datas.Rows)
                {
                    model.FetchData(data);
                    AddToReminders(model);
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        private void AddToReminders(ReminderModel model)
        {
            try
            {
                ReminderModel temp = new ReminderModel();
                temp.GuiD = model.GuiD;
                temp.Content = model.Content;
                temp.ReminderTime = model.ReminderTime;
                temp.Contact = model.Contact;
                temp.Record = model.Record;
                temp.RecordPath = model.RecordPath;
                temp.Ring = model.Ring;
                Reminders.Add(temp);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 提醒语音播报回调
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isReport"></param>
        private void ReminderFromReporter(string content,bool isReport)
        {
            ReminderReporter?.Invoke(content, isReport);
        }
    }
}
