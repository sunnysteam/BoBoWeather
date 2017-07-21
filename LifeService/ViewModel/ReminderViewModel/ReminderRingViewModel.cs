using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model.ReminderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LifeService.ViewModel.ReminderViewModel
{
    internal class ReminderRingViewModel : ViewModelBase
    {
        private DeletgateCommand<Window> _loadedCommand;
        private DeletgateCommand<Window> _closeButtonCommand;
        private DeletgateCommand<Window> _nextButtonCommand;
        private Timer _reportTimer = null;
        private Window _mainWindow;
        private string _reminderTimeRing;
        //private MediaPlayer _player = new MediaPlayer();
        private int _internal = 0;
        private bool _isTime = true;
        /// <summary>
        /// 提醒语音播报
        /// </summary>
        public Action<string, bool> ReminderReporter;

        public ReminderRingViewModel(ReminderModel model)
        {
            try
            {
                Content = model.Content;
                string[] reminderTimes = model.ReminderTime.Split(' ');
                string time;
                string[] times;
                if (reminderTimes[0].Equals("只提醒一次"))
                {
                    time = reminderTimes[1] + " " + reminderTimes[2];
                }
                else
                {
                    time = reminderTimes[reminderTimes.Length - 1];
                }
                times = time.Split(' ');
                if (times[0].Contains("/"))
                {
                    if (times[1].Contains("午"))
                    {
                        if (times[1].Equals("上午"))
                        {
                            ReminderTime = times[0] + " " + times[2] + "am";
                        }
                        else
                        {
                            ReminderTime = times[0] + " " + times[2] + "pm";
                        }
                    }
                    else
                    {
                        ReminderTime = times[0] + " " + times[1];
                    }
                }
                else
                {
                    if (times[0].Contains("午"))
                    {
                        if (times[0].Equals("上午"))
                        {
                            ReminderTime = times[1] + "am";
                        }
                        else
                        {
                            ReminderTime = times[1] + "pm";
                        }
                    }
                    else
                    {
                        ReminderTime = times[0];
                    }
                }
                //ReminderTime = ReminderTime.Substring(0, ReminderTime.Length - 3);
                _reminderTimeRing = ReminderTime;
                Ring = model.Ring;
                if (string.IsNullOrEmpty(model.Contact))
                {
                    Label = "备忘提醒";
                }
                else
                {
                    Label = "远程提醒";
                }
                TimeTick();
                //_player.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Media/Music/" + model.Ring, UriKind.RelativeOrAbsolute));
                //_player.Play();
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 唯一值
        /// </summary>
        private string _guid;

        public string GuiD
        {
            get { return _guid; }
            set
            {
                _guid = value;
                OnPropertyChanged("GuiD");
            }
        }

        /// <summary>
        /// 提醒时间
        /// </summary>
        private string _reminderTime;

        public string ReminderTime
        {
            get { return _reminderTime; }
            set
            {
                _reminderTime = value;
                OnPropertyChanged("ReminderTime");
            }
        }


        /// <summary>
        /// 提醒内容
        /// </summary>
        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        /// <summary>
        /// 标签
        /// </summary>
        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        /// <summary>
        /// 下一次时间
        /// </summary>
        private string _nextReminderTime;

        public string NextReminderTime
        {
            get { return _nextReminderTime; }
            set
            {
                _nextReminderTime = value;
                OnPropertyChanged("NextReminderTime");
            }
        }

        /// <summary>
        /// 铃声
        /// </summary>
        private string _ring;

        public string Ring
        {
            get { return _ring; }
            set
            {
                _ring = value;
                OnPropertyChanged("Ring");
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new DeletgateCommand<Window>(Loaded, CanLoaded);
                }
                return _loadedCommand;

            }
        }

        public ICommand NextButtonCommand
        {
            get
            {
                if (_nextButtonCommand == null)
                {
                    _nextButtonCommand = new DeletgateCommand<Window>(NextButton, CanNextButton);
                }
                return _nextButtonCommand;

            }
        }

        public ICommand CloseButtonCommand
        {
            get
            {
                if (_closeButtonCommand == null)
                {
                    _closeButtonCommand = new DeletgateCommand<Window>(CloseButton, CanCloseButton);
                }
                return _closeButtonCommand;

            }
        }

        private void Loaded(Window element)
        {
            _mainWindow = element;
            ReminderReporter?.Invoke(Content, true);
        }

        private void NextButton(Window element)
        {
            _isTime = false;
            ReminderReporter?.Invoke(Content, false);
            _reminderTimeRing = NextReminderTime;
            _mainWindow = element;
            _mainWindow.Hide();
            //_player.Stop();
        }

        private void CloseButton(Window element)
        {
            _reportTimer?.Dispose();
            ReminderReporter?.Invoke(Content, false);
            //_player.Close();
            element.Close();
        }

        private static bool CanLoaded(Window element)
        {
            return true;
        }

        private static bool CanNextButton(Window element)
        {
            return true;
        }

        private static bool CanCloseButton(Window element)
        {
            return true;
        }

        /// <summary>
        /// 计时
        /// </summary>
        private void TimeTick()
        {
            _reportTimer = new Timer(new TimerCallback(callback =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                       () =>
                       {
                           try
                           {
                               //下次提醒时间
                               NextReminderTime = (DateTime.Now + new TimeSpan(0, 9, 0)).ToString();
                               NextReminderTime = NextReminderTime.Substring(0, NextReminderTime.Length - 3);
                               //现在时间
                               //AlarmTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm");
                               //当前时间与提醒时间比对
                               if ((DateTime.Now - Convert.ToDateTime(_reminderTimeRing)).TotalSeconds <= 5 && (DateTime.Now - Convert.ToDateTime(_reminderTimeRing)).TotalSeconds >= 0)
                               {

                                   if (_mainWindow != null)
                                   {
                                       string[] times = Convert.ToDateTime(_reminderTimeRing).ToString().Split(' ');
                                       if (times[1].Contains("午"))
                                       {
                                           if (times[1].Equals("上午"))
                                           {
                                               ReminderTime = times[0] + " " + times[2] + "am";
                                           }
                                           else
                                           {
                                               ReminderTime = times[0] + " " + times[2] + "pm";
                                           }
                                       }
                                       else
                                       {
                                           ReminderTime = times[0] + " " + times[1];
                                       }
                                       ReminderTime = ReminderTime.Substring(0, ReminderTime.Length - 3);
                                       ReminderReporter?.Invoke(Content, true);
                                       _mainWindow.Show();
                                       _isTime = true;
                                       //_player.Play();
                                   }
                               }
                               if (_isTime)
                               {
                                   _internal++;
                                   if (_internal % 15 == 0)
                                   {
                                       ReminderReporter?.Invoke(Content, true);
                                   }
                                   else if (_internal > 44)//44*3000 = 132s
                                   {
                                       ReminderReporter?.Invoke(Content, false);
                                       //_player.Stop();
                                       //_player.Close();
                                       _mainWindow.Close();
                                       _isTime = false;
                                       _internal = 0;
                                   }
                               }
                           }
                           catch (Exception ex)
                           {
                               LogWriter.Instance.Error(ex);
                           }
                       });
            }), null, 0, 3000); //如:一天执行一次 1000 * 60 * 60 * 24
        }
    }
}
