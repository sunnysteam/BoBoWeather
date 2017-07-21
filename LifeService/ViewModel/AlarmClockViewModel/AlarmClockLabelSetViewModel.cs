using LifeService.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockLabelSetViewModel : ViewModelBase
    {
        private DeletgateCommand<Grid> _clickBackCommand;
        private DeletgateCommand<TextBox> _textChangedCommand;
        private DeletgateCommand<TextBox> _labelDeleteCommand;
        public Action<string> BackToMainView;

        public AlarmClockLabelSetViewModel(string label)
        {
            Label = label;
            Length = Label.Length.ToString();
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

        #region 命令
        public ICommand ClickBackCommand
        {
            get
            {
                if (_clickBackCommand == null)
                {
                    _clickBackCommand = new DeletgateCommand<Grid>(ClickBack, CanClickBack);
                }
                return _clickBackCommand;

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

        public ICommand LabelDeleteCommand
        {
            get
            {
                if (_labelDeleteCommand == null)
                {
                    _labelDeleteCommand = new DeletgateCommand<TextBox>(LabelDelete, CanLabelDelete);
                }
                return _labelDeleteCommand;

            }
        }
        #endregion

        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="element"></param>
        private void ClickBack(Grid element)
        {
            BackToMainView(Label);
        }

        /// <summary>
        /// 字符变化数
        /// </summary>
        /// <param name="element"></param>
        private void TextChanged(TextBox element)
        {
            if (element.Text.Length > 0)
            {
                Length = element.Text.Length.ToString();
            }
            else
            {
                Length = "0";
            }
            element.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="element"></param>
        private void LabelDelete(TextBox element)
        {
            Label = "";
        }

        private static bool CanClickBack(Grid element)
        {
            return true;
        }

        private static bool CanTextChanged(TextBox element)
        {
            return true;
        }

        private static bool CanLabelDelete(TextBox element)
        {
            return true;
        }
    }
}
