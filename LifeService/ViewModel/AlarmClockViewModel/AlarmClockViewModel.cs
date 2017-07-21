using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LifeService.Command;
using LifeService.Model;
using LifeService.Model.AlarmClockModel;
using GalaSoft.MvvmLight.Threading;
using System.Threading.Tasks;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockViewModel : ViewModelBase
    {
        private DeletgateCommand<UserControl> _deleteButtonCommand;
        private DeletgateCommand<ScrollViewer> _checkedBoxCommand;
        private DeletgateCommand<Grid> _mouseLeftButtonDownEditCommand;
        public Action<ListBoxItem> DeleteFromListBox;
        public Action<AlarmClockModel> EditAlarmClock;

        public AlarmClockViewModel()
        {
            IEnumerable<string> scoreQuery =
                from item in EnumberHelper.EnumToList<WeekEnum>()
                select item.EnumName;
            _weekList = scoreQuery.ToList();
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
        /// 闹钟时间
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
        /// 闹钟日期
        /// </summary>
        private string _alarmDate;

        public string AlarmDate
        {
            get { return _alarmDate; }
            set
            {
                _alarmDate = value;
                SetMainLabel();
                OnPropertyChanged("AlarmDate");
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
                SetMainLabel();
                OnPropertyChanged("Label");
            }
        }

       
        /// <summary>
        /// 标签+时间
        /// </summary>
        private string _mainLabel = "闹钟";
        public string MainLabel
        {
            get { return _mainLabel; }
            set
            {
                _mainLabel = value;
                OnPropertyChanged("MainLabel");
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
        /// 铃声路径
        /// </summary>
        private string _ringpath;

        public string RingPath
        {
            get { return _ringpath; }
            set
            {
                _ringpath = value;
                OnPropertyChanged("RingPath");
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
                _isRepetition = value;
                OnPropertyChanged("IsRepetition");
            }
        }

        /// <summary>
        /// 是否开启
        /// </summary>
        private string _isOpen;
        public string IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyChanged("IsOpen");
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
        /// 周
        /// </summary>
        private List<string> _weekList;

        public List<string> WeekList
        {
            get { return _weekList; }
            set
            {
                _weekList = value;
                OnPropertyChanged("WeekList");
            }
        }

        #region 命令
        public ICommand DeleteButtonCommand
        {
            get
            {
                if (_deleteButtonCommand == null)
                {
                    _deleteButtonCommand = new DeletgateCommand<UserControl>(DeleteButton, CanDeleteButton);
                }
                return _deleteButtonCommand;

            }
        }

        public ICommand CheckedBoxCommand
        {
            get
            {
                if (_checkedBoxCommand == null)
                {
                    _checkedBoxCommand = new DeletgateCommand<ScrollViewer>(CheckedBox, CanCheckedBox);
                }
                return _checkedBoxCommand;

            }
        }

        public ICommand MouseLeftButtonDownEditCommand
        {
            get
            {
                if (_mouseLeftButtonDownEditCommand == null)
                {
                    _mouseLeftButtonDownEditCommand = new DeletgateCommand<Grid>(MouseLeftButtonDownEdit, CanMouseLeftButtonDownEdit);
                }
                return _mouseLeftButtonDownEditCommand;

            }
        }
        #endregion

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="element"></param>
        private void DeleteButton(UserControl element)
        {
            DeleteFromListBox((element.Parent as ListBoxItem));
        }

        /// <summary>
        /// 是否开启闹钟
        /// </summary>
        /// <param name="element"></param>
        private void CheckedBox(ScrollViewer element)
        {
            Task.Factory.StartNew(() =>
            {
                AlarmClockModel model = new AlarmClockModel();
                model.GuiD = GuiD;
                model.AlarmTime = string.Format("{0:H:mm}", Convert.ToDateTime(Format + " " + AlarmTime));
                model.AlarmDate = AlarmDate;
                model.Label = Label;
                model.Ring = Ring;
                model.RingPath = RingPath;
                model.IsRepetition = IsRepetition;
                model.IsOpen = IsOpen == "True" ? "1" : "0";
                DataManager.Instance.UpdateTables(model);
                MainAlarmClockManager.Instance.AlarmClocks.Where((temp) => { return temp.GuiD == model.GuiD; }).First().IsOpen = model.IsOpen;
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    LogWriter.Instance.Error(result.Exception);
                }
            });
        }

        /// <summary>
        /// 编辑闹钟
        /// </summary>
        /// <param name="element"></param>
        private void MouseLeftButtonDownEdit(Grid element)
        {
            try
            {
                AlarmClockModel model = new AlarmClockModel();
                model.GuiD = GuiD;
                model.AlarmTime = Format + " " + AlarmTime;
                model.AlarmDate = AlarmDate;
                model.Label = Label;
                model.Ring = Ring;
                model.RingPath = RingPath;
                model.IsRepetition = IsRepetition;
                model.IsOpen = IsOpen == "True" ? "1" : "0";
                EditAlarmClock(model);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        #region 命令条件
        private static bool CanDeleteButton(UserControl element)
        {
            return true;
        }

        private static bool CanCheckedBox(ScrollViewer element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownEdit(Grid element)
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 设置label
        /// </summary>
        private void SetMainLabel()
        {
            if (string.IsNullOrEmpty(AlarmDate))
            {
                MainLabel = Label;
            }
            else if(string.IsNullOrEmpty(Label))
            {
                MainLabel = AlarmDate;
            }
            else
            {
                MainLabel = Label + "，" + AlarmDate;
            }
        }

        /// <summary>
        /// 删除闹钟
        /// </summary>
        public void DeleteData()
        {
            try
            {
                AlarmClockModel model = new AlarmClockModel();
                model.GuiD = GuiD;
                MainAlarmClockManager.Instance.AlarmClocks.RemoveAll((obj) => { return obj.GuiD == model.GuiD; });
                DataManager.Instance.DeleteTables(model);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 取数据
        /// </summary>
        /// <param name="model"></param>
        public void FetchData(AlarmClockModel model)
        {
            try
            {
                GuiD = model.GuiD;
                AlarmTime = model.AlarmTime;
                AlarmDate = model.AlarmDate;
                string[] times = Convert.ToDateTime(model.AlarmTime).ToShortTimeString().Split(' ');
                if (times[0].Contains("午"))
                {
                    Format = times[0];
                    AlarmTime = times[1];
                }
                else
                {
                    AlarmTime = times[0];
                }
                Label = model.Label;
                Ring = model.Ring;
                RingPath = model.RingPath;
                IsRepetition = model.IsRepetition;
                IsOpen = model.IsOpen == "1" ? "True" : "False";
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }
    }
}
