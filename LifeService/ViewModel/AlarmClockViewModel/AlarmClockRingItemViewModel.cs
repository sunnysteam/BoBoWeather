using LifeService.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockRingItemViewModel : ViewModelBase
    {

        private DeletgateCommand<Grid> _mouseLeftButtonDownCommand;
        public Action<string> BackToMainView;

        public AlarmClockRingItemViewModel(string label)
        {
            Label = label;
        }

        /// <summary>
        /// 标题
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

        private void MouseLeftButtonDown(Grid element)
        {
            BackToMainView(Label);
        }

        private static bool CanMouseLeftButtonDown(Grid element)
        {
            return true;
        }
    }
}
