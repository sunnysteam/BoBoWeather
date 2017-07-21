using System;
using System.Windows.Controls;
using System.Windows.Input;
using LifeService.Command;
using LifeService.Model;
using LifeService.Model.ReminderModel;

namespace LifeService.ViewModel.ReminderViewModel
{
    internal class MemoReminderViewModel: ViewModelBase
    {
        private DeletgateCommand<Grid> _mouseLeftButtonDownCommand;

        public Action<ReminderModel> EditReminder;

        public MemoReminderViewModel()
        {
            
        }

        #region 命令

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

        /// <summary>
        /// 编辑闹钟
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDown(Grid element)
        {
            ReminderModel model = new ReminderModel();
            model.GuiD = GuiD;
            model.Content = Content;
            model.Contact = Contact;
            model.ReminderTime = ReminderTime;
            model.Record = Record;
            model.RecordPath = RecordPath;
            model.Ring = Ring;
            EditReminder(model);
        }

        private static bool CanSaveButton(Grid element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDown(Grid element)
        {
            return true;
        }

        #endregion


        #region 属性

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
        /// 显示内容
        /// </summary>
        private string _labelContent;

        public string LabelContent
        {
            get { return _labelContent; }
            set
            {
                _labelContent = value;
                OnPropertyChanged("LabelContent");
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
        /// 录音
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

        #endregion

        public void FetchData(ReminderModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Contact))
                {
                    GuiD = model.GuiD;
                    ReminderTime = Convert.ToDateTime(model.ReminderTime).ToString();
                    ReminderTime = ReminderTime.Substring(0, ReminderTime.Length - 3);
                    Content = model.Content;
                    Contact = model.Contact;
                    Record = model.Record;
                    RecordPath = model.RecordPath;
                    Ring = model.Ring;
                    if (!string.IsNullOrEmpty(Contact))
                    {
                        Label = Contact + "，" + ReminderTime;
                    }
                    else
                    {
                        Label = ReminderTime;
                    }
                    if (!string.IsNullOrEmpty(Content) && Content.Length > 13)
                    {
                        LabelContent = Content.Substring(0, 13) + "...";
                    }
                    else
                    {
                        LabelContent = Content;
                    }
                }
                else
                {
                    GuiD = model.GuiD;
                    ReminderTime = model.ReminderTime;
                    Content = model.Content;
                    Contact = model.Contact;
                    Record = model.Record;
                    RecordPath = model.RecordPath;
                    Ring = model.Ring;
                    Label = Contact + "，" + ReminderTime;
                    if (!string.IsNullOrEmpty(Content) && Content.Length > 13)
                    {
                        LabelContent = Content.Substring(0, 13) + "...";
                    }
                    else
                    {
                        LabelContent = Content;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }
    }
}
