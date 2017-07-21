using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LifeService.Model.AlarmClockModel;
using LifeService.View.AlarmClockView;
using GalaSoft.MvvmLight.Threading;
using LifeService.Model;
using System.Data;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class MainAlarmClockManager
    {
        private static MainAlarmClockManager _instance;
        private static readonly object Padlock = new object();
        private Timer _reportTimer = null;
        public static MainAlarmClockManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new MainAlarmClockManager();
                    }
                    return _instance;
                }
            }
        }

        private MainAlarmClockManager()
        {
            FetchData();
            AlarmClockMonitor();
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

        private List<AlarmClockRingView> _alarmClocksWindows = new List<AlarmClockRingView>();
        private List<AlarmClockModel> _alarmClocks = new List<AlarmClockModel>();
        public List<AlarmClockModel> AlarmClocks
        {
            get { return _alarmClocks; }
            set { _alarmClocks = value; }
        }

        /// <summary>
        /// 闹钟监视
        /// </summary>
        private void AlarmClockMonitor()
        {
            _reportTimer = new Timer(new TimerCallback(callback =>
            {
                try
                {
                    foreach (var clockModel in AlarmClocks)
                    {
                        if (clockModel.IsOpen.Equals("1"))
                        {
                            if (clockModel.AlarmDate.Equals("每天"))
                            {
                                string week = "周一 周二 周三 周四 周五 周六 周日";
                                if (week.Contains(TodayWeek))
                                {
                                    MatchTime(clockModel);
                                }
                            }
                            else if (clockModel.AlarmDate.Equals("工作日"))
                            {
                                string week = "周一 周二 周三 周四 周五";
                                if (week.Contains(TodayWeek))
                                {
                                    MatchTime(clockModel);
                                }
                            }
                            else if (clockModel.AlarmDate.Equals("周末"))
                            {
                                string week = "周六 周日";
                                if (week.Contains(TodayWeek))
                                {
                                    MatchTime(clockModel);
                                }
                            }
                            else if (clockModel.AlarmDate.Equals(""))
                            {
                                MatchTime(clockModel);
                            }
                            else
                            {
                                if (clockModel.AlarmDate.Contains(TodayWeek))
                                {
                                    MatchTime(clockModel);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Instance.Error(ex);
                }
            }), null, 0, 10000); //一天执行一次=1000 * 60 * 60 * 24
        }

        /// <summary>
        /// 时间匹配
        /// </summary>
        /// <param name="clockModel"></param>
        private void MatchTime(AlarmClockModel clockModel)
        {
            try
            {
                TimeSpan date = DateTime.Now - (Convert.ToDateTime(clockModel.AlarmTime));
                if (date.TotalSeconds <= 10 && date.TotalSeconds >= 0)
                {
                    if (string.IsNullOrEmpty(clockModel.AlarmDate))
                    {
                        clockModel.IsOpen = "0";
                        DataManager.Instance.UpdateTables(clockModel);
                        AlarmClocks.Where(temp => temp.GuiD == clockModel.GuiD).First().IsOpen = "0";
                    }
                    AlarmClockRing(clockModel);
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 响铃
        /// </summary>
        /// <param name="clockModel"></param>
        private void AlarmClockRing(AlarmClockModel clockModel)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            AlarmClockRingView view = new AlarmClockRingView();
                            view.DataContext = new AlarmClockRingViewModel(clockModel);
                            _alarmClocksWindows.Add(view);
                            view.Show();
                        });
        }

        public void CloseAlarmClock()
        {
            try
            {
                if (_alarmClocksWindows != null && _alarmClocksWindows.Count > 0)
                {
                    foreach (var item in _alarmClocksWindows)
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
                AlarmClockModel model = new AlarmClockModel();
                DataTable datas = DataManager.Instance.SelectTables(model);
                AlarmClocks.Clear();
                foreach (DataRow data in datas.Rows)
                {
                    model.FetchData(data);
                    AddToAlarmClocks(model);
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 添加到闹钟集合
        /// </summary>
        /// <param name="model"></param>
        private void AddToAlarmClocks(AlarmClockModel model)
        {
            try
            {
                AlarmClockModel temp = new AlarmClockModel();
                temp.AlarmDate = model.AlarmDate;
                temp.AlarmTime = model.AlarmTime;
                temp.GuiD = model.GuiD;
                temp.IsOpen = model.IsOpen;
                temp.IsRepetition = model.IsRepetition;
                temp.Label = model.Label;
                temp.Ring = model.Ring;
                temp.RingPath = model.RingPath;
                AlarmClocks.Add(temp);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

    }
}
