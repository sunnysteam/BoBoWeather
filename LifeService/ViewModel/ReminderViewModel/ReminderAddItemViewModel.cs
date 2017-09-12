using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model;
using LifeService.Model.ReminderModel;
using LifeService.View.AlarmClockView;
using LifeService.ViewModel.AlarmClockViewModel;
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
using System.Windows.Media.Imaging;

namespace LifeService.ViewModel.ReminderViewModel
{
    internal class ReminderAddItemViewModel : ViewModelBase
    {
        private DeletgateCommand<RollNum> _loadRollDataCommand;
        private DeletgateCommand<Grid> _mouseLeftButtonDownDateCommand;
        private DeletgateCommand<Viewbox> _mouseLeftButtonDownRingCommand;
        private DeletgateCommand<Image> _mouseLeftButtonDownCommand;
        private DeletgateCommand<Grid> _mouseLeftButtonDownDeleteCommand;
        private DeletgateCommand<TextBox> _textChangedCommand;
        private DeletgateCommand<TextBox> _loadedTextBoxCommand;
        private DeletgateCommand<Grid> _loadedCommand;
        private DeletgateCommand<Border> _loadedEditCommand;
        private DeletgateCommand<Grid> _cancelCommand;
        private DeletgateCommand<Grid> _saveDataCommand;
        private bool _isFormat = false;//是否24小时制
        private bool _isDate = false;//是否打开日期面板
        private Grid _lastElement;
        private bool _isFirstLoaded = true;//是否初次加载
        private bool _isMemoReminder = true;//是否备忘提醒
        private bool _isEdit = false;//是否编辑
        public Action<string> BackToMainView;
        private ReminderModel _lastReminder;
        private Viewbox _mainViewbox;
        private UIElement _lastUIElement;

        private RollNum _minutes;
        private RollNum _hours;
        private RollNum _format;
        private RollNum _days;
        private RollNum _months;
        private RollNum _years;

        private string minutes { get; set; }

        private string hours { get; set; }

        private string format { get; set; }

        private string days { get; set; }

        private string months { get; set; }

        private string years { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="function"></param>
        public ReminderAddItemViewModel(string function)
        {
            try
            {
                WordIcon = new BitmapImage(WordIconPath);
                VoiceIcon = new BitmapImage(VoiceIconPath);
                TitleContent = "添加提醒";
                //if(DateTime.Now.ToString())
                //string date = string.Format("{0:yyyy/M/d tt H:mm}", DateTime.Now);
                DateContent = DateTime.Now.ToString();

                //判断是否24小时制
                if (DateContent.Contains("午"))
                {
                    string[] temp = DateContent.Split(' ');
                    string[] datestring = temp[0].Split('/');
                    years = datestring[0];
                    months = datestring[1];
                    days = datestring[2];
                    format = temp[1];
                    string[] timestring = temp[2].Split(':');
                    hours = timestring[0];
                    minutes = timestring[1];
                }
                else
                {
                    string[] temp = DateContent.Split(' ');
                    string[] datestring = temp[0].Split('/');
                    years = datestring[0];
                    months = datestring[1];
                    days = datestring[2];
                    string[] timestring = temp[1].Split(':');
                    hours = timestring[0];
                    minutes = timestring[1];
                }
                DateContent = DateContent.Substring(0, DateContent.Length - 3);
                if (function.Equals("MemoReminder"))
                {
                    _isMemoReminder = true;
                }
                if (function.Equals("RemoteReminder"))
                {
                    ContactsList = DataManager.Instance.Contacts;
                    _isMemoReminder = false;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        public ReminderAddItemViewModel(ReminderModel model)
        {
            try
            {
                _lastReminder = model;
                TitleContent = "详细信息";
                _isEdit = true;
                GuiD = model.GuiD;
                Content = model.Content;
                Length = string.IsNullOrEmpty(Content) ? "0" : Content.Length.ToString();
                Contact = model.Contact;
                ReminderTime = model.ReminderTime;
                Record = model.Record;
                RecordPath = model.RecordPath;
                Ring = model.Ring;
                if (string.IsNullOrEmpty(Contact))
                {
                    _isMemoReminder = true;
                }
                else
                {
                    _isMemoReminder = false;
                }
                if (_isMemoReminder)
                {
                    DateContent = Convert.ToDateTime(model.ReminderTime).ToString();
                    DateContent = DateContent.Substring(0, DateContent.Length - 3);
                    //DateContent = DateTime.Now.ToString();
                    if (DateContent.Contains("午"))
                    {
                        string[] temp = DateContent.Split(' ');
                        string[] datestring = temp[0].Split('/');
                        years = datestring[0];
                        months = datestring[1];
                        days = datestring[2];
                        format = temp[1];
                        string[] timestring = temp[2].Split(':');
                        hours = timestring[0];
                        minutes = timestring[1];
                    }
                    else
                    {
                        string[] temp = DateContent.Split(' ');
                        string[] datestring = temp[0].Split('/');
                        years = datestring[0];
                        months = datestring[1];
                        days = datestring[2];
                        string[] timestring = temp[1].Split(':');
                        hours = timestring[0];
                        minutes = timestring[1];
                    }
                }
                else
                {
                    DateContent = model.ReminderTime;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        #region 属性
        /// <summary>
        /// 日期内容
        /// </summary>
        private string _dateContent;
        public string DateContent
        {
            get { return _dateContent; }
            set
            {
                _dateContent = value;
                OnPropertyChanged("DateContent");
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
        /// 联系人
        /// </summary>
        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        /// <summary>
        /// 语音
        /// </summary>
        private string _record;
        public string Record
        {
            get { return _record; }
            set
            {
                _record = value;
                OnPropertyChanged("Record");
            }
        }

        /// <summary>
        /// 语音路径
        /// </summary>
        private string _recordPath;
        public string RecordPath
        {
            get { return _recordPath; }
            set
            {
                _recordPath = value;
                OnPropertyChanged("RecordPath");
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

        /// <summary>
        /// 文字
        /// </summary>
        private Uri _wordIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/ReminderImages/word_nor.png", UriKind.RelativeOrAbsolute);
        public Uri WordIconPath
        {
            get { return _wordIconPath; }
            set
            {
                _wordIconPath = value;
                OnPropertyChanged("WordIconPath");
            }
        }

        /// <summary>
        /// 语音
        /// </summary>
        private Uri _voiceIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/ReminderImages/voice_nor.png", UriKind.RelativeOrAbsolute);
        public Uri VoiceIconPath
        {
            get { return _voiceIconPath; }
            set
            {
                _voiceIconPath = value;
                OnPropertyChanged("VoiceIconPath");
            }
        }

        /// <summary>
        /// 文字图标
        /// </summary>
        private ImageSource _wordIcon;
        public ImageSource WordIcon
        {
            get { return _wordIcon; }
            set
            {
                _wordIcon = value;
                OnPropertyChanged("WordIcon");
            }
        }

        /// <summary>
        /// 语音图标
        /// </summary>
        private ImageSource _voiceIcon;
        public ImageSource VoiceIcon
        {
            get { return _voiceIcon; }
            set
            {
                _voiceIcon = value;
                OnPropertyChanged("VoiceIcon");
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
        /// 内容
        /// </summary>
        private string _content = "";
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
        /// 长度
        /// </summary>
        private string _length = "0";
        public string Length
        {
            get { return _length; }
            set
            {
                _length = value;
                OnPropertyChanged("Length");
            }
        }

        /// <summary>
        /// 联系人列表
        /// </summary>
        private List<ContactsModel> _contactsList;
        public List<ContactsModel> ContactsList
        {
            get { return _contactsList; }
            set
            {
                _contactsList = value;
                OnPropertyChanged("ContactsList");
            }
        }
        #endregion

        #region 命令
        public ICommand LoadRollDataCommand
        {
            get
            {
                if (_loadRollDataCommand == null)
                {
                    _loadRollDataCommand = new DeletgateCommand<RollNum>(LoadRollData, CanLoadRollData);
                }
                return _loadRollDataCommand;

            }
        }

        public ICommand MouseLeftButtonDownDateCommand
        {
            get
            {
                if (_mouseLeftButtonDownDateCommand == null)
                {
                    _mouseLeftButtonDownDateCommand = new DeletgateCommand<Grid>(MouseLeftButtonDownDate, CanMouseLeftButtonDownDate);
                }
                return _mouseLeftButtonDownDateCommand;

            }
        }

        public ICommand MouseLeftButtonDownCommand
        {
            get
            {
                if (_mouseLeftButtonDownCommand == null)
                {
                    _mouseLeftButtonDownCommand = new DeletgateCommand<Image>(MouseLeftButtonDown, CanMouseLeftButtonDown);
                }
                return _mouseLeftButtonDownCommand;

            }
        }

        public ICommand MouseLeftButtonDownDeleteCommand
        {
            get
            {
                if (_mouseLeftButtonDownDeleteCommand == null)
                {
                    _mouseLeftButtonDownDeleteCommand = new DeletgateCommand<Grid>(MouseLeftButtonDownDelete, CanMouseLeftButtonDownDelete);
                }
                return _mouseLeftButtonDownDeleteCommand;

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

        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new DeletgateCommand<Grid>(Loaded, CanLoaded);
                }
                return _loadedCommand;

            }
        }

        public ICommand LoadedEditCommand
        {
            get
            {
                if (_loadedEditCommand == null)
                {
                    _loadedEditCommand = new DeletgateCommand<Border>(LoadedEdit, CanLoadedEdit);
                }
                return _loadedEditCommand;

            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DeletgateCommand<Grid>(Cancel, CanCancel);
                }
                return _cancelCommand;

            }
        }

        public ICommand TextChangedCommand
        {
            get
            {
                if (_textChangedCommand == null)
                {
                    _textChangedCommand = new DeletgateCommand<TextBox>(TextChanged, CanTextChanged);
                }
                return _textChangedCommand;

            }
        }

        public ICommand LoadedTextBoxCommand
        {
            get
            {
                if (_loadedTextBoxCommand == null)
                {
                    _loadedTextBoxCommand = new DeletgateCommand<TextBox>(LoadedTextBox, CanLoadedTextBox);
                }
                return _loadedTextBoxCommand;

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
        #endregion

        /// <summary>
        /// 加载小时
        /// </summary>
        /// <param name="element"></param>
        private void LoadRollData(RollNum element)
        {
            switch (element.Name)
            {
                case "Roll_Years":
                    LoadRollYears(element);
                    break;
                case "Roll_Months":
                    LoadRollMonths(element);
                    break;
                case "Roll_Days":
                    LoadRollDays(element);
                    break;
                case "Roll_Format":
                    LoadRollFormat(element);
                    break;
                case "Roll_Hours":
                    LoadRollHours(element);
                    break;
                case "Roll_Minutes":
                    LoadRollMinutes(element);
                    break;
            }
        }

        private void Loaded(Grid element)
        {
            if (element.Name.Equals("dateGrid"))
            {
                if (_isFirstLoaded)
                {
                    _lastElement = element;
                    (element.Parent as Grid).Children.Remove(element);
                    _isFirstLoaded = false;
                }
            }
            if (element.Name.Equals("mainGrid"))
            {
                if (_isEdit)
                {
                    //显示内容框
                    element.Children[0].Visibility = Visibility.Visible;
                    element.Children[1].Visibility = Visibility.Visible;
                    element.Children[2].Visibility = Visibility.Visible;
                    element.Children[3].Visibility = Visibility.Hidden;
                }

                ////移除联系人框
                //if (_isMemoReminder)
                //{
                //    int index = 0;
                //    foreach (var item in element.Children)
                //    {
                //        if (item is Grid)
                //        {
                //            if ((item as Grid).Name.Equals("contactGrid"))
                //                index = element.Children.IndexOf(item as Grid);
                //        }
                //    }
                //    element.Children.RemoveAt(index);
                //}
            }

        }

        private void LoadedEdit(Border element)
        {
            //显示或隐藏联系人框
            if (_isMemoReminder)
            {
                if (element.Name.Equals("contactBorder"))
                {
                    element.Visibility = Visibility.Hidden;
                }
                if (element.Name.Equals("ringBorder"))
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (element.Name.Equals("contactBorder"))
                {
                    element.Visibility = Visibility.Visible;
                    element.IsEnabled = false;
                }
                if (element.Name.Equals("ringBorder"))
                {
                    element.Visibility = Visibility.Hidden;
                }
                if (element.Name.Equals("timeBorder"))
                {
                    element.IsEnabled = false;
                }
                foreach (var item in (element.Child as Grid).Children)
                {
                    if (item is Image)
                        (item as Image).Visibility = Visibility.Hidden;
                }
            }
            //显示或隐藏删除框
            if (_isEdit)
            {
                if (element.Name.Equals("deleteBorder"))
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (element.Name.Equals("deleteBorder"))
                {
                    element.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Cancel(Grid element)
        {
            BackToMainView("Cancel");
        }

        /// <summary>
        /// 编辑提醒内容
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDown(Image element)
        {
            string name = element.Name;
            Task.Factory.StartNew(() =>
            {
                if (name.Equals("wordImage"))
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            WordIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/ReminderImages/word_sel.png", UriKind.RelativeOrAbsolute);
                            WordIcon = new BitmapImage(WordIconPath);
                        });
                    Thread.Sleep(200);
                    DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            //显示内容框
                            ((((element.Parent as Border).Parent as Grid).Parent as Border).Parent as Grid).Children[0].Visibility = Visibility.Visible;
                            ((((element.Parent as Border).Parent as Grid).Parent as Border).Parent as Grid).Children[1].Visibility = Visibility.Visible;
                            ((((element.Parent as Border).Parent as Grid).Parent as Border).Parent as Grid).Children[2].Visibility = Visibility.Visible;
                            ((((element.Parent as Border).Parent as Grid).Parent as Border).Parent as Grid).Children[3].Visibility = Visibility.Hidden;
                        });
                }
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    LogWriter.Instance.Error(result.Exception);
                }
            });

        }

        /// <summary>
        /// 删除提醒
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDownDelete(Grid element)
        {
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            element.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#153f5f"));
                        });
                ReminderManager.Instance.Reminders.RemoveAll((obj) => { return obj.GuiD == GuiD; });
                DataManager.Instance.DeleteTables(_lastReminder);
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

        /// <summary>
        /// 编辑时间
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDownDate(Grid element)
        {
            try
            {
                if (element.Name.Equals("timeGrid"))
                {
                    //打开或关闭日期选择面板
                    if (_isDate)
                    {
                        element.Background = new SolidColorBrush(Colors.Transparent);
                        DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
                        ((element.Parent as Border).Parent as Grid).Children.Remove(_lastElement);
                        _lastElement.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);

                        _isDate = false;
                    }
                    else
                    {
                        element.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#153f5f"));
                        ((element.Parent as Border).Parent as Grid).Children.Add(_lastElement);
                        DoubleAnimation dLoginFadeOut = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.2)));
                        _lastElement.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);

                        _isDate = true;
                    }
                }
                if (_years != null)
                {
                    years = _years.Currentitem;
                    months = _months.Currentitem;
                    days = _days.Currentitem;
                    format = _format.Currentitem;
                    hours = _hours.Currentitem;
                    minutes = _minutes.Currentitem;
                    DateContent = _years.Currentitem + "/" + _months.Currentitem + "/" + _days.Currentitem + " " + _format.Currentitem + " " + _hours.Currentitem + ":" + _minutes.Currentitem;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
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
        /// 存储提醒
        /// </summary>
        /// <param name="elememt"></param>
        private void SaveData(Grid elememt)
        {
            try
            {
                if (TitleContent.Equals("添加提醒"))//添加提醒
                {
                    ReminderModel model = new ReminderModel();
                    GuiD = Guid.NewGuid().ToString().Replace("-", "");
                    model.GuiD = GuiD;
                    if (_years != null)
                    {
                        ReminderTime = string.Format("{0:yyyy/M/d H:mm}", Convert.ToDateTime(_years.Currentitem + "/" + _months.Currentitem + "/" + _days.Currentitem
                        + " " + _format.Currentitem + " " + _hours.Currentitem + ":" + _minutes.Currentitem));
                    }
                    else
                    {
                        ReminderTime = string.Format("{0:yyyy/M/d H:mm}", Convert.ToDateTime(years + "/" + months + "/" + days + " " + format + " " + hours + ":" + minutes));
                    }
                    model.ReminderTime = ReminderTime;
                    model.Content = Content;
                    model.Contact = Contact;
                    model.Record = Record;
                    model.RecordPath = RecordPath;
                    model.Ring = Ring;
                    DataManager.Instance.InsertTables(model);
                    ReminderManager.Instance.Reminders.Add(model);
                    BackToMainView("Cancel");
                }
                else if (TitleContent.Equals("详细信息"))//修改提醒
                {
                    int index = ReminderManager.Instance.Reminders.IndexOf(ReminderManager.Instance.Reminders.Where(temp => temp.GuiD == _lastReminder.GuiD).First());
                    if (_years != null)
                    {
                        ReminderTime = string.Format("{0:yyyy/M/d H:mm}", Convert.ToDateTime(_years.Currentitem + "/" + _months.Currentitem + "/" + _days.Currentitem
                        + " " + _format.Currentitem + " " + _hours.Currentitem + ":" + _minutes.Currentitem));
                    }
                    else
                    {
                        ReminderTime = string.Format("{0:yyyy/M/d H:mm}", Convert.ToDateTime(years + "/" + months + "/" + days + " " + format + " " + hours + ":" + minutes));
                    }
                    _lastReminder.ReminderTime = ReminderTime;
                    _lastReminder.Content = Content;
                    _lastReminder.Contact = Contact;
                    _lastReminder.Record = Record;
                    _lastReminder.RecordPath = RecordPath;
                    _lastReminder.Ring = Ring;
                    DataManager.Instance.UpdateTables(_lastReminder);
                    ReminderManager.Instance.Reminders[index] = _lastReminder;
                    BackToMainView("Cancel");
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 字符变化数
        /// </summary>
        /// <param name="element"></param>
        private void TextChanged(TextBox element)
        {
            Length = Content.Length.ToString();
            element.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// 字符变化数
        /// </summary>
        /// <param name="element"></param>
        private void LoadedTextBox(TextBox element)
        {
            if (_isMemoReminder)
            {
                element.IsEnabled = true;
            }
            else
            {
                element.IsEnabled = false;
            }
        }

        #region 命令条件
        private static bool CanMouseLeftButtonDownRing(Viewbox element)
        {
            return true;
        }
        private static bool CanLoaded(Grid element)
        {
            return true;
        }

        private static bool CanLoadedEdit(Border element)
        {
            return true;
        }

        private static bool CanCancel(Grid element)
        {
            return true;
        }

        private static bool CanLoadRollData(RollNum element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDown(Image element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownDelete(Grid element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownDate(Grid element)
        {
            return true;
        }

        private static bool CanTextChanged(TextBox element)
        {
            return true;
        }

        private static bool CanLoadedTextBox(TextBox element)
        {
            return true;
        }

        private static bool CanSaveData(Grid element)
        {
            return true;
        }
        #endregion

        #region 加载日期时间
        private void LoadRollMinutes(RollNum element)
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
                element.CurrentIndex = element.Items.IndexOf(minutes);
            }
            else
            {
                minutes = element.Currentitem;
            }
        }

        private void LoadRollHours(RollNum element)
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
                element.CurrentIndex = element.Items.IndexOf(hours);
            }
            else
            {
                hours = element.Currentitem;
            }
        }

        private void LoadRollFormat(RollNum element)
        {
            _format = element;
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
                    (element.Parent as Canvas).Width = 60;
                    element.CurrentIndex = element.Items.IndexOf(format);
                }
                else
                {
                    element.Items.Clear();
                    (element.Parent as Canvas).Width = 40;
                }
            }
            else
            {
                format = element.Currentitem;
            }
        }

        private void LoadRollDays(RollNum element)
        {
            _days = element;
            if (string.IsNullOrEmpty(element.Currentitem))
            {
                List<string> ls = new List<string>();
                for (int i = 1; i <= 31; i++)
                {
                    ls.Add(i.ToString());
                }
                element.Items = ls;
                element.CurrentIndex = element.Items.IndexOf(days);
            }
            else
            {
                days = element.Currentitem;
            }
        }

        private void LoadRollMonths(RollNum element)
        {
            _months = element;
            if (string.IsNullOrEmpty(element.Currentitem))
            {
                List<string> ls = new List<string>();
                for (int i = 1; i <= 12; i++)
                {
                    ls.Add(i.ToString());
                }
                element.Items = ls;
                element.CurrentIndex = element.Items.IndexOf(months);
            }
            else
            {
                months = element.Currentitem;
            }
        }

        private void LoadRollYears(RollNum element)
        {
            _years = element;
            if (string.IsNullOrEmpty(element.Currentitem))
            {
                List<string> ls = new List<string>();
                int year = DateTime.Now.Year;
                for (int i = 0; i < 5; i++)
                {
                    ls.Add(year.ToString());
                    year++;
                }
                element.Items = ls;
                element.CurrentIndex = element.Items.IndexOf(years);
            }
            else
            {
                years = element.Currentitem;
            }
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
        /// 铃声设置完成回调
        /// </summary>
        /// <param name="ring"></param>
        private void BackFromRingView(string ring)
        {
            Ring = ring;
            _mainViewbox.Child = _lastUIElement;
        }
        #endregion
    }
}
