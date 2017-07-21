using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model.ReminderModel;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LifeService.ViewModel.ReminderViewModel
{
    internal class MemoReminderRingViewModel:ViewModelBase
    {
        private string _alarmTimeRing;
        private Window _mainWindow;
        private Timer _reportTimer = null;

        private DeletgateCommand<Window> _closeButtonCommand;
        private DeletgateCommand<Window> _nextButtonCommand;
        private DeletgateCommand<Window> _reminderRingWindowMouseDownCommand;

        public MemoReminderRingViewModel(ReminderModel reminder)
        {
            //_alarmTimeRing = Convert.ToDateTime(reminder.Time).ToString("HH") + ":" + Convert.ToDateTime(reminder.Time).ToString("mm");
            //AlarmTime = _alarmTimeRing;
            //Content = reminder.ContentText;
            TimeTick();
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
        /// 内容
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

        public ICommand ReminderRingWindowMouseDownCommand
        {
            get
            {
                if (_reminderRingWindowMouseDownCommand == null)
                {
                    _reminderRingWindowMouseDownCommand = new DeletgateCommand<Window>(ReminderRingWindowMouseDown, CanReminderRingWindowMouseDownCommand);
                }
                return _reminderRingWindowMouseDownCommand;

            }
        }

        private void NextButton(Window element)
        {
            _alarmTimeRing = NextAlarmTime.Substring(7, 5);
            _mainWindow = element;
            _mainWindow.Hide();
        }

        private void CloseButton(Window element)
        {
            element.Close();
        }

        private void ReminderRingWindowMouseDown(Window element)
        {
            if (element != null)
            {
                element.DragMove();
            }
        }

        private static bool CanNextButton(Window element)
        {
            return true;
        }

        private static bool CanCloseButton(Window element)
        {
            return true;
        }

        private static bool CanReminderRingWindowMouseDownCommand(Window element)
        {
            return true;
        }

        private void TimeTick()
        {
            _reportTimer = new Timer(new TimerCallback(callback =>
            {
                try
                {
                    //下次提醒时间
                    NextAlarmTime = (DateTime.Now + new TimeSpan(0, 2, 0)).ToString("HH") + ":" + (DateTime.Now + new TimeSpan(0, 2, 0)).ToString("mm");
                    //现在时间
                    AlarmTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm");
                    //当前时间与提醒时间比对
                    if ((DateTime.Now - Convert.ToDateTime(_alarmTimeRing)).TotalSeconds <= 5 && (DateTime.Now - Convert.ToDateTime(_alarmTimeRing)).TotalSeconds >= 0)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            if (_mainWindow != null)
                            {
                                _mainWindow.Show();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Instance.Error(ex);
                }
            }), null, 0, 3000); //一天执行一次 1000 * 60 * 60 * 24
        }
    }
}
