using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.View.AlarmClockView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockRingSetViewModel : ViewModelBase
    {
        private DeletgateCommand<ListBox> _loadDataCommand;
        private DeletgateCommand<ListBox> _unLoadDataCommand;
        private DeletgateCommand<Grid> _clickBackCommand;
        private ListBox _mainListBox;
        public Action<string> BackToMainView;
        private MediaPlayer _player;
        private string _path = AppDomain.CurrentDomain.BaseDirectory + "/Media/Music/";

        public AlarmClockRingSetViewModel(string ring)
        {
            _listBoxItems = new ObservableCollection<ListBoxItem>();
            _player = new MediaPlayer();
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


        /// <summary>
        /// 闹钟集合
        /// </summary>
        private ObservableCollection<ListBoxItem> _listBoxItems;
        public ObservableCollection<ListBoxItem> ListBoxItems
        {
            get { return _listBoxItems; }
            set { _listBoxItems = value; }
        }

        public ICommand LoadDataCommand
        {
            get
            {
                if (_loadDataCommand == null)
                {
                    _loadDataCommand = new DeletgateCommand<ListBox>(LoadData, CanLoadData);
                }
                return _loadDataCommand;

            }
        }

        public ICommand UnLoadDataCommand
        {
            get
            {
                if (_unLoadDataCommand == null)
                {
                    _unLoadDataCommand = new DeletgateCommand<ListBox>(UnLoadData, CanUnLoadData);
                }
                return _unLoadDataCommand;

            }
        }

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


        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="element"></param>
        private void ClickBack(Grid element)
        {
            _player.Stop();
            BackToMainView(Label);
        }

        /// <summary>
        /// listbox加载
        /// </summary>
        /// <param name="element"></param>
        private void LoadData(ListBox element)
        {
            _mainListBox = element;
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            
                            string[] filenames = Directory.GetFiles(_path);  //获取该文件夹下面的所有文件名
                            //List<string> names = new List<string>();
                            foreach (var item in filenames)
                            {
                                //names.Add(item.Substring(item.LastIndexOf("/") + 1));
                                AddRing(item.Substring(item.LastIndexOf("/") + 1));
                            }
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
        /// listbox加载
        /// </summary>
        /// <param name="element"></param>
        private void UnLoadData(ListBox element)
        {
            _player.Stop();
            _player.Close();
        }

        private static bool CanLoadData(ListBox element)
        {
            return true;
        }

        private static bool CanUnLoadData(ListBox element)
        {
            return true;
        }

        private static bool CanClickBack(Grid element)
        {
            return true;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <param name="element"></param>
        private void AddRing(string ring)
        {
            try
            {
                ListBoxItem item = new ListBoxItem();
                //加载资源字典
                string packUri = @"/LifeService;component/WindowDictionary.xaml";
                ResourceDictionary myResourceDictionary = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
                item.SetValue(ListBoxItem.StyleProperty, myResourceDictionary["OnlyBackgroundListBoxItemStyle"]);

                AlarmClockRingItemView clockView = new AlarmClockRingItemView();
                AlarmClockRingItemViewModel clockViewModel = new AlarmClockRingItemViewModel(ring);
                clockViewModel.BackToMainView = ArrivedAtMainView;
                clockView.DataContext = clockViewModel;
                item.Content = clockView;
                ListBoxItems.Add(item);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        private void ArrivedAtMainView(string target)
        {
            if (_player != null)
            {
                _player.Stop();
                Label = target;
                _player.Open(new Uri(_path + Label, UriKind.Relative));
                _player.Play();
            }
        }
    }
}
