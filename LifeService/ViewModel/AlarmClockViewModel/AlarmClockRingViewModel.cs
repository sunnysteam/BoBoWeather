using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model.AlarmClockModel;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockRingViewModel : ViewModelBase
    {
        private DeletgateCommand<Window> _loadedCommand;
        private DeletgateCommand<Window> _closeButtonCommand;
        private DeletgateCommand<Window> _nextButtonCommand;
        private DeletgateCommand<Window> _alarmClockWindowMouseDownCommand;
        private Timer _reportTimer = null;
        private Window _mainWindow;
        private string _alarmTimeRing;
        private MediaPlayer _player = new MediaPlayer();
        private int _internal = 0;
        private bool _isTime = true;

        public AlarmClockRingViewModel(AlarmClockModel time)
        {
            try
            {
                IsRepetition = time.IsRepetition;
                string[] times = Convert.ToDateTime(time.AlarmTime).ToShortTimeString().Split(' ');
                if (times[0].Contains("午"))
                {
                    if (times[0].Equals("上午"))
                    {
                        AlarmTime = times[1] + "am";
                    }
                    else
                    {
                        AlarmTime = times[1] + "pm";
                    }
                }
                else
                {
                    AlarmTime = times[0];
                }
                _alarmTimeRing = time.AlarmTime;
                Label = time.Label;
                TimeTick();
                _player.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Media/Music/" + time.Ring, UriKind.RelativeOrAbsolute));
                _player.Play();
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
            
        }

        #region

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

        public ICommand AlarmClockWindowMouseDownCommand
        {
            get
            {
                if (_alarmClockWindowMouseDownCommand == null)
                {
                    _alarmClockWindowMouseDownCommand = new DeletgateCommand<Window>(AlarmClockWindowMouseDown, CanAlarmClockWindowMouseDownCommand);
                }
                return _alarmClockWindowMouseDownCommand;

            }
        }

        private void NextButton(Window element)
        {
            _isTime = false;
            _alarmTimeRing = NextAlarmTime.Substring(7, 5);
            _mainWindow = element;
            _mainWindow.Hide();
            _player.Stop();
        }

        private void Loaded(Window element)
        {
            _mainWindow = element;
        }

        private void CloseButton(Window element)
        {
            _player.Close();
            element.Close();
        }

        private void AlarmClockWindowMouseDown(Window element)
        {
            element.DragMove();
        }

        private static bool CanNextButton(Window element)
        {
            return true;
        }

        private static bool CanLoaded(Window element)
        {
            return true;
        }

        private static bool CanCloseButton(Window element)
        {
            return true;
        }

        private static bool CanAlarmClockWindowMouseDownCommand(Window element)
        {
            return true;
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
        /// 闹铃时间
        /// </summary>
        private string _alarmTime;

        public string AlarmTime
        {
            get { return _alarmTime; }
            set
            {
                _alarmTime = value;
                OnPropertyChanged("AlarmTime");
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
        private string _nextAlarmTime;

        public string NextAlarmTime
        {
            get { return _nextAlarmTime; }
            set
            {
                _nextAlarmTime = string.Format("下一次提醒{0}" + value, Environment.NewLine);
                OnPropertyChanged("NextAlarmTime");
            }
        }

        /// <summary>
        /// 是否稍后提醒
        /// </summary>
        private string _isRepetition;

        public string IsRepetition
        {
            get { return _isRepetition; }
            set
            {
                if (value.Equals("1"))
                {
                    _isRepetition = "Visible";
                }
                else
                {
                    _isRepetition = "Hidden";
                }
                OnPropertyChanged("IsRepetition");
            }
        }

        #endregion

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
                                NextAlarmTime = (DateTime.Now + new TimeSpan(0, 9, 0)).ToString("HH") + ":" + (DateTime.Now + new TimeSpan(0, 9, 0)).ToString("mm");
                                //现在时间
                                //AlarmTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm");
                                //当前时间与提醒时间比对
                                if ((DateTime.Now - Convert.ToDateTime(_alarmTimeRing)).TotalSeconds <= 5 && (DateTime.Now - Convert.ToDateTime(_alarmTimeRing)).TotalSeconds >= 0)
                                {
                                    if (_mainWindow != null)
                                    {
                                        string[] times = Convert.ToDateTime(_alarmTimeRing).ToShortTimeString().Split(' ');
                                        if (times[0].Contains("午"))
                                        {
                                            if (times[0].Equals("上午"))
                                            {
                                                AlarmTime = times[1] + "am";
                                            }
                                            else
                                            {
                                                AlarmTime = times[1] + "pm";
                                            }
                                        }
                                        else
                                        {
                                            AlarmTime = times[0];
                                        }
                                        _mainWindow.Show();
                                        _isTime = true;
                                        _player.Play();
                                    }
                                }
                                if (_isTime)
                                {
                                    _internal++;
                                    if (_internal > 20)
                                    {
                                        _player.Stop();
                                        _player.Close();
                                        _mainWindow.Close();
                                        _isTime = false;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogWriter.Instance.Error(ex);
                            }
                        });
            }), null, 0, 3000); //一天执行一次 1000 * 60 * 60 * 24
        }
    }
}
