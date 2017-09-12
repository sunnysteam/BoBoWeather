using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model;
using LifeService.Model.AlarmClockModel;
using LifeService.View.AlarmClockView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockAddItemViewModel : ViewModelBase
    {
        private DeletgateCommand<RollNum> _loadRollHoursDataCommand;
        private DeletgateCommand<RollNum> _loadRollMinutesDataCommand;
        private DeletgateCommand<RollNum> _loadRollDataFormatCommand;
        private DeletgateCommand<Grid> _mouseLeftButtonDownCommand;
        private DeletgateCommand<Grid> _saveDataCommand;
        private DeletgateCommand<Viewbox> _mouseLeftButtonDownRepetionCommand;
        private DeletgateCommand<Viewbox> _mouseLeftButtonDownLabelCommand;
        private DeletgateCommand<Viewbox> _mouseLeftButtonDownRingCommand;
        private DeletgateCommand<Border> _loadedDeleteButtonCommand;

        private Viewbox _mainViewbox;
        private UIElement _lastUIElement;

        public Action<string> BackToMainView;
        private bool _isFormat;
        private RollNum _hours;
        private RollNum _minutes;
        private RollNum format;
        private AlarmClockModel _lastAlarmClock;

        public AlarmClockAddItemViewModel(string function)
        {
            try
            {
                TitleContent = function;
                string[] times = Convert.ToDateTime(DateTime.Now).ToShortTimeString().Split(' ');
                if (times[0].Contains("午"))
                {
                    Format = times[0];
                    string[] time = times[1].Split(':');
                    Hours = time[0];
                    Minutes = time[1];
                }
                else
                {
                    string[] time = times[0].Split(':');
                    Hours = time[0];
                    Minutes = time[1];
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        public AlarmClockAddItemViewModel(AlarmClockModel model)
        {
            try
            {
                _lastAlarmClock = model;
                TitleContent = "编辑闹钟";
                GuiD = model.GuiD;
                string[] times = Convert.ToDateTime(model.AlarmTime).ToShortTimeString().Split(' ');
                if (times[0].Contains("午"))
                {
                    Format = times[0];
                    string[] time = times[1].Split(':');
                    Hours = time[0];
                    Minutes = time[1];
                }
                else
                {
                    string[] time = times[0].Split(':');
                    Hours = time[0];
                    Minutes = time[1];
                }
                Days = model.AlarmDate == "" ? "永不" : model.AlarmDate;
                Label = model.Label;
                Ring = model.Ring;
                IsRepetition = model.IsRepetition == "1" ? "True" : "False";
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
        /// 小时
        /// </summary>
        private string _hoursstring;
        public string Hours
        {
            get { return _hoursstring; }
            set
            {
                _hoursstring = value;
                OnPropertyChanged("Hours");
            }
        }

        /// <summary>
        /// 分钟
        /// </summary>
        private string _minutesstring;
        public string Minutes
        {
            get { return _minutesstring; }
            set
            {
                _minutesstring = value;
                OnPropertyChanged("Minutes");
            }
        }

        /// <summary>
        /// 时间格式
        /// </summary>
        private string _format;
        public string Format
        {
            get { return _format; }
            set
            {
                _format = value;
                OnPropertyChanged("Format");
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        private string _titleContent;
        public string TitleContent
        {
            get { return _titleContent; }
            set
            {
                _titleContent = value;
                OnPropertyChanged("TitleContent");
            }
        }

        /// <summary>
        /// 重复
        /// </summary>
        private string _days = "永不";
        public string Days
        {
            get { return _days; }
            set
            {
                _days = value;
                OnPropertyChanged("Days");
            }
        }

        /// <summary>
        /// 标签
        /// </summary>
        private string _label = "闹钟";
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
        /// 稍后提醒
        /// </summary>
        private string _isRepetition = "True";
        public string IsRepetition
        {
            get { return _isRepetition; }
            set
            {
                _isRepetition = value;
                OnPropertyChanged("IsRepetition");
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

        #region 命令
        public ICommand LoadRollHoursDataCommand
        {
            get
            {
                if (_loadRollHoursDataCommand == null)
                {
                    _loadRollHoursDataCommand = new DeletgateCommand<RollNum>(LoadRollHoursData, CanLoadRollHoursData);
                }
                return _loadRollHoursDataCommand;

            }
        }

        public ICommand LoadRollMinutesDataCommand
        {
            get
            {
                if (_loadRollMinutesDataCommand == null)
                {
                    _loadRollMinutesDataCommand = new DeletgateCommand<RollNum>(LoadRollMinutesData, CanLoadRollMinutesData);
                }
                return _loadRollMinutesDataCommand;

            }
        }

        public ICommand LoadRollDataFormatCommand
        {
            get
            {
                if (_loadRollDataFormatCommand == null)
                {
                    _loadRollDataFormatCommand = new DeletgateCommand<RollNum>(LoadRollDataFormat, CanLoadRollDataFormat);
                }
                return _loadRollDataFormatCommand;

            }
        }

        public ICommand MouseLeftButtonDownCommand
        {
            get
            {
                if (_mouseLeftButtonDownCommand == null)
                {
                    _mouseLeftButtonDownCommand = new DeletgateCommand<Grid>(MouseLeftButtonDown, CanMouseLeftButtonDown);
                }
                return _mouseLeftButtonDownCommand;

            }
        }

        public ICommand SaveDataCommand
        {
            get
            {
                if (_saveDataCommand == null)
                {
                    _saveDataCommand = new DeletgateCommand<Grid>(SaveData, CanSaveData);
                }
                return _saveDataCommand;

            }
        }

        public ICommand LoadedDeleteButtonCommand
        {
            get
            {
                if (_loadedDeleteButtonCommand == null)
                {
                    _loadedDeleteButtonCommand = new DeletgateCommand<Border>(LoadedDeleteButton, CanLoadedDeleteButton);
                }
                return _loadedDeleteButtonCommand;

            }
        }

        public ICommand MouseLeftButtonDownRepetionCommand
        {
            get
            {
                if (_mouseLeftButtonDownRepetionCommand == null)
                {
                    _mouseLeftButtonDownRepetionCommand = new DeletgateCommand<Viewbox>(MouseLeftButtonDownRepetion, CanMouseLeftButtonDownRepetion);
                }
                return _mouseLeftButtonDownRepetionCommand;

            }
        }

        public ICommand MouseLeftButtonDownLabelCommand
        {
            get
            {
                if (_mouseLeftButtonDownLabelCommand == null)
                {
                    _mouseLeftButtonDownLabelCommand = new DeletgateCommand<Viewbox>(MouseLeftButtonDownLabel, CanMouseLeftButtonDownLabel);
                }
                return _mouseLeftButtonDownLabelCommand;

            }
        }

        public ICommand MouseLeftButtonDownRingCommand
        {
            get
            {
                if (_mouseLeftButtonDownRingCommand == null)
                {
                    _mouseLeftButtonDownRingCommand = new DeletgateCommand<Viewbox>(MouseLeftButtonDownRing, CanMouseLeftButtonDownRing);
                }
                return _mouseLeftButtonDownRingCommand;

            }
        }
        #endregion

        /// <summary>
        /// 加载小时
        /// </summary>
        /// <param name="element"></param>
        private void LoadRollHoursData(RollNum element)
        {
            try
            {
                _hours = element;
                if (string.IsNullOrEmpty(element.Currentitem))
                {
                    List<string> ls = new List<string>();
                    if (_isFormat)
                    {
                        for (int i = 0; i < 13; i++)
                        {
                            ls.Add(i.ToString());
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            ls.Add(i.ToString());
                        }
                    }
                    element.Items = ls;
                    if (!string.IsNullOrEmpty(Hours))
                    {
                        element.CurrentIndex = element.Items.IndexOf(Hours);
                    }
                }
                else
                {
                    Hours = element.Currentitem;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }

        }


        /// <summary>
        /// 加载分钟
        /// </summary>
        /// <param name="element"></param>
        private void LoadRollMinutesData(RollNum element)
        {
            try
            {
                _minutes = element;
                if (string.IsNullOrEmpty(element.Currentitem))
                {
                    List<string> ls = new List<string>();
                    for (int i = 0; i < 60; i++)
                    {
                        if (i < 10)
                        {
                            ls.Add("0" + i.ToString());
                        }
                        else
                        {
                            ls.Add(i.ToString());
                        }
                    }
                    element.Items = ls;
                    if (!string.IsNullOrEmpty(Minutes))
                    {
                        element.CurrentIndex = element.Items.IndexOf(Minutes);
                    }
                }
                else
                {
                    Minutes = element.Currentitem;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 加载格式
        /// </summary>
        /// <param name="element"></param>
        private void LoadRollDataFormat(RollNum element)
        {
            try
            {
                format = element;
                if (string.IsNullOrEmpty(element.Currentitem))
                {
                    if (DateTime.Now.ToShortTimeString().Contains("上午") || DateTime.Now.ToShortTimeString().Contains("下午"))
                    {
                        _isFormat = true;
                    }
                    else
                    {
                        _isFormat = false;
                    }
                    if (_isFormat)
                    {
                        List<string> ls = new List<string>();
                        ls.Add("上午");
                        ls.Add("下午");
                        element.Items = ls;
                        (element.Parent as Canvas).Width = 80;
                    }
                    else
                    {
                        element.Items.Clear();
                        (element.Parent as Canvas).Width = 0;
                    }
                    if (!string.IsNullOrEmpty(Format))
                    {
                        element.CurrentIndex = element.Items.IndexOf(Format);
                    }
                }
                else
                {
                    Format = element.Currentitem;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 删除/取消按钮
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDown(Grid element)
        {
            if (element.Name.Equals("deleteGrid"))
            {
                Task.Factory.StartNew(() =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                            () =>
                            {
                                element.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#153f5f"));
                            });
                    MainAlarmClockManager.Instance.AlarmClocks.RemoveAll((obj) => { return obj.GuiD == GuiD; });
                    DataManager.Instance.DeleteTables(_lastAlarmClock);
                    Thread.Sleep(200);
                    DispatcherHelper.CheckBeginInvokeOnUI(
                            () =>
                            {
                                BackToMainView("Delete");
                            });
                }).ContinueWith(result =>
                {
                    if (result.IsFaulted)
                    {
                        LogWriter.Instance.Error(result.Exception);
                    }
                });
            }
            else
            {
                BackToMainView("Cancel");
            }
        }

        /// <summary>
        /// 加载初始功能类型
        /// </summary>
        /// <param name="element"></param>
        private void LoadedDeleteButton(Border element)
        {
            if (TitleContent.Equals("添加闹钟"))
            {
                element.Visibility = Visibility.Hidden;
            }
            else if (TitleContent.Equals("编辑闹钟"))
            {
                element.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 选择星期
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDownRepetion(Viewbox element)
        {
            _mainViewbox = element;
            _lastUIElement = _mainViewbox.Child;
            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            dLoginFadeOut.Completed += DRepetionFadeOut_Completed;
            element.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
        }

        /// <summary>
        /// 设置标签
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDownLabel(Viewbox element)
        {
            _mainViewbox = element;
            _lastUIElement = _mainViewbox.Child;
            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            dLoginFadeOut.Completed += DLabelFadeOut_Completed;
            element.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
        }

        /// <summary>
        /// 设置铃声
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDownRing(Viewbox element)
        {
            _mainViewbox = element;
            _lastUIElement = _mainViewbox.Child;
            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            dLoginFadeOut.Completed += DRingFadeOut_Completed;
            element.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
        }

        /// <summary>
        /// 存储闹钟
        /// </summary>
        /// <param name="elememt"></param>
        private void SaveData(Grid elememt)
        {
            try
            {
                if (TitleContent.Equals("添加闹钟"))
                {
                    AlarmClockModel model = new AlarmClockModel();
                    GuiD = Guid.NewGuid().ToString().Replace("-", "");
                    model.GuiD = GuiD;
                    Hours = _hours.Currentitem;
                    Minutes = _minutes.Currentitem;
                    Format = format.Currentitem;
                    Time = string.Format("{0:H:mm}", Convert.ToDateTime(Format + " " + Hours + ":" + Minutes));
                    model.AlarmTime = Time;
                    model.AlarmDate = Days == "永不" ? "" : Days;
                    model.Label = Label;
                    model.IsRepetition = IsRepetition == "True" ? "1" : "0";
                    model.IsOpen = "1";
                    model.Ring = Ring;
                    DataManager.Instance.InsertTables(model);
                    MainAlarmClockManager.Instance.AlarmClocks.Add(model);
                    BackToMainView("Cancel");
                }
                else if (TitleContent.Equals("编辑闹钟"))
                {
                    int index = MainAlarmClockManager.Instance.AlarmClocks.IndexOf(MainAlarmClockManager.Instance.AlarmClocks.Where(temp => temp.GuiD == _lastAlarmClock.GuiD).First());
                    Hours = _hours.Currentitem;
                    Minutes = _minutes.Currentitem;
                    Format = format.Currentitem;
                    Time = string.Format("{0:H:mm}", Convert.ToDateTime(Format + " " + Hours + ":" + Minutes));
                    _lastAlarmClock.AlarmTime = Time;
                    _lastAlarmClock.AlarmDate = Days == "永不" ? "" : Days;
                    _lastAlarmClock.Label = Label;
                    _lastAlarmClock.IsOpen = "1";
                    _lastAlarmClock.IsRepetition = IsRepetition == "True" ? "1" : "0";
                    _lastAlarmClock.Ring = Ring;
                    DataManager.Instance.UpdateTables(_lastAlarmClock);
                    MainAlarmClockManager.Instance.AlarmClocks[index] = _lastAlarmClock;
                    BackToMainView("Cancel");
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        #region 命令条件
        private static bool CanLoadRollHoursData(RollNum element)
        {
            return true;
        }

        private static bool CanLoadRollMinutesData(RollNum element)
        {
            return true;
        }

        private static bool CanLoadRollDataFormat(RollNum element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDown(Grid element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownRepetion(Viewbox element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownLabel(Viewbox element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownRing(Viewbox element)
        {
            return true;
        }

        private static bool CanSaveData(Grid element)
        {
            return true;
        }

        private static bool CanLoadedDeleteButton(Border element)
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DRepetionFadeOut_Completed(object sender, EventArgs e)
        {
            AlarmClockWeekSetView view = new AlarmClockWeekSetView();
            AlarmClockWeekSetViewModel viewModel = new AlarmClockWeekSetViewModel(Days);
            viewModel.BackToMainView += BackFromWeekView;
            view.DataContext = viewModel;
            _mainViewbox.Child = view;
        }

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLabelFadeOut_Completed(object sender, EventArgs e)
        {
            AlarmClockLabelSetView view = new AlarmClockLabelSetView();
            AlarmClockLabelSetViewModel viewModel = new AlarmClockLabelSetViewModel(Label);
            viewModel.BackToMainView += BackFromLabelView;
            view.DataContext = viewModel;
            _mainViewbox.Child = view;
        }

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DRingFadeOut_Completed(object sender, EventArgs e)
        {
            AlarmClockRingSetView view = new AlarmClockRingSetView();
            AlarmClockRingSetViewModel viewModel = new AlarmClockRingSetViewModel(Ring);
            viewModel.BackToMainView += BackFromRingView;
            view.DataContext = viewModel;
            _mainViewbox.Child = view;
        }

        /// <summary>
        /// 标签设置完成回调
        /// </summary>
        /// <param name="label"></param>
        private void BackFromLabelView(string label)
        {
            Label = label;
            _mainViewbox.Child = _lastUIElement;
        }

        /// <summary>
        /// 铃声设置完成回调
        /// </summary>
        /// <param name="label"></param>
        private void BackFromRingView(string ring)
        {
            Ring = ring;
            _mainViewbox.Child = _lastUIElement;
        }

        /// <summary>
        /// 星期设置完成回调
        /// </summary>
        /// <param name="temp"></param>
        private void BackFromWeekView(List<WeekDay> temp)
        {
            try
            {
                if (temp.Count == 7)
                {
                    Days = "每天";
                }
                else
                {
                    var isWordDay = from item in temp
                                    where item.Index < 6
                                    select item;
                    var isWeekend = from item in temp
                                    where item.Index > 5
                                    select item;
                    if (isWordDay.Count() == 5 && temp.Count == 5)
                    {
                        Days = "工作日";
                    }
                    else if (isWeekend.Count() == 2 && temp.Count == 2)
                    {
                        Days = "周末";
                    }
                    else
                    {
                        Days = "";
                        foreach (var item in temp)
                        {
                            Days += item.Day.Substring(1) + " ";
                        }
                        if (Days.Length == 0)
                        {
                            Days = "永不";
                        }
                        else
                        {
                            Days = Days.Substring(0, Days.Count() - 1);
                        }
                    }
                }
                _mainViewbox.Child = _lastUIElement;
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }
    }
}
