using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model;
using LifeService.Model.AlarmClockModel;
using LifeService.View.AlarmClockView;
using System.Collections.ObjectModel;
using System;
using System.Threading;
using System.Windows.Media.Animation;

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class MainAlarmClockViewModel: ViewModelBase
    {
        private DeletgateCommand<Border> _addButtonCommand;
        private DeletgateCommand<Border> _loadedBorderCommand;
        private DeletgateCommand<ListBox> _loadDataCommand;
        private DeletgateCommand<Window> _deactivatedCommand;
        private DeletgateCommand<Window> _windowDragCommand;
        private DeletgateCommand<ListBox> _loadEditDataCommand;
        private DeletgateCommand<Window> _loadedCommand;
        private Border _mainBorder;
        private UIElement _lastUIElement;
        private ListBox _mainListBox;
        private AlarmClockModel _temp;
        private Window _mainWindow;

        public MainAlarmClockViewModel()
        {
            _listBoxItems = new ObservableCollection<ListBoxItem>();
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

        /// <summary>
        /// 编辑/完成按钮
        /// </summary>
        private string _buttonContent = "编辑";

        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
                OnPropertyChanged("ButtonContent");
            }
        }

        #region 命令
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

        public ICommand LoadedBorderCommand
        {
            get
            {
                if (_loadedBorderCommand == null)
                {
                    _loadedBorderCommand = new DeletgateCommand<Border>(LoadedBorder, CanLoadedBorder);
                }
                return _loadedBorderCommand;

            }
        }

        public ICommand WindowDragCommand
        {
            get
            {
                if (_windowDragCommand == null)
                {
                    _windowDragCommand = new DeletgateCommand<Window>(WindowDrag, CanWindowDrag);
                }
                return _windowDragCommand;

            }
        }

        public ICommand DeactivatedCommand
        {
            get
            {
                if (_deactivatedCommand == null)
                {
                    _deactivatedCommand = new DeletgateCommand<Window>(Deactivated, CanDeactivated);
                }
                return _deactivatedCommand;

            }
        }

        public ICommand AddButtonCommand
        {
            get
            {
                if (_addButtonCommand == null)
                {
                    _addButtonCommand = new DeletgateCommand<Border>(AddButton, CanAddButton);
                }
                return _addButtonCommand;

            }
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

        public ICommand LoadEditDataCommand
        {
            get
            {
                if (_loadEditDataCommand == null)
                {
                    _loadEditDataCommand = new DeletgateCommand<ListBox>(LoadEditData, CanLoadEditData);
                }
                return _loadEditDataCommand;

            }
        }
        #endregion

        /// <summary>
        /// 主窗口加载
        /// </summary>
        /// <param name="element"></param>
        private void Loaded(Window element)
        {
            _mainWindow = element;
            LifeServicePlugin.Instance.CloseLifeServiceAction += CloseAlarmClock;
        }

        /// <summary>
        /// 窗口移动
        /// </summary>
        /// <param name="element"></param>
        private void WindowDrag(Window element)
        {
            element.DragMove();
        }

        /// <summary>
        /// 失去焦点后关闭窗口
        /// </summary>
        /// <param name="element"></param>
        private void Deactivated(Window element)
        {
            element.Hide();
            LifeServicePlugin.Instance.CloseLifeService();
        }

        /// <summary>
        /// 主border加载
        /// </summary>
        /// <param name="element"></param>
        private void LoadedBorder(Border element)
        {
            _mainBorder = element;
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="element"></param>
        private void AddButton(Border element)
        {
            try
            {
                _mainBorder = element;
                _lastUIElement = element.Child;
                DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
                dLoginFadeOut.Completed += DLoginFadeOut_Completed;
                element.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
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
                            FetchData(element);
                        });
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    LogWriter.Instance.Error(result.Exception);
                }
            });
        }

        #region 命令条件
        private static bool CanLoaded(Window element)
        {
            return true;
        }

        private void LoadEditData(ListBox element)
        {
            FetchEditData(element, ButtonContent);
        }

        private static bool CanWindowDrag(Window element)
        {
            return true;
        }

        private static bool CanDeactivated(Window element)
        {
            return true;
        }

        private static bool CanAddButton(Border element)
        {
            return true;
        }

        private static bool CanLoadedBorder(Border element)
        {
            return true;
        }

        private static bool CanLoadData(ListBox element)
        {
            return true;
        }
        private static bool CanLoadEditData(ListBox element)
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLoginFadeOut_Completed(object sender, EventArgs e)
        {
            AlarmClockAddItemView view = new AlarmClockAddItemView();
            AlarmClockAddItemViewModel viewModel = new AlarmClockAddItemViewModel("添加闹钟");
            viewModel.BackToMainView += BackToMainView;
            view.DataContext = viewModel;
            _mainBorder.Child = view;
        }

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLoginFadeIn_Completed(object sender, EventArgs e)
        {
            AlarmClockAddItemView view = new AlarmClockAddItemView();
            AlarmClockAddItemViewModel viewModel = new AlarmClockAddItemViewModel(_temp);
            viewModel.BackToMainView += BackToMainView;
            view.DataContext = viewModel;
            _mainBorder.Child = view;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <param name="element"></param>
        private void FetchData(ListBox element)
        {
            try
            {
                ListBoxItems.Clear();
                foreach (var item in MainAlarmClockManager.Instance.AlarmClocks)
                {
                    AddNewReminder(element, item);
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 获取需修改数据
        /// </summary>
        /// <param name="element"></param>
        /// <param name="buttonContent"></param>
        private void FetchEditData(ListBox element, string buttonContent)
        {
            try
            {
                if (ButtonContent.Equals("编辑"))
                {
                    ButtonContent = "完成";
                    ListBoxItems.Clear();

                    foreach (AlarmClockModel model in MainAlarmClockManager.Instance.AlarmClocks)
                    {
                        AddNewEditReminder(element, model);
                    }
                }
                else if (ButtonContent.Equals("完成"))
                {
                    ButtonContent = "编辑";
                    ListBoxItems.Clear();

                    foreach (AlarmClockModel model in MainAlarmClockManager.Instance.AlarmClocks)
                    {
                        AddNewReminder(element, model);
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 添加闹钟
        /// </summary>
        /// <param name="element"></param>
        /// <param name="model"></param>
        private void AddNewReminder(ListBox element, AlarmClockModel model)
        {
            try
            {
                ListBoxItem item = new ListBoxItem();
                //加载资源字典
                string packUri = @"/LifeService;component/WindowDictionary.xaml";
                ResourceDictionary myResourceDictionary = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
                item.SetValue(ListBoxItem.StyleProperty, myResourceDictionary["NoBackgroundListBoxItemStyle"]);

                AlarmClockView clockView = new AlarmClockView(element);
                AlarmClockViewModel clockViewModel = new AlarmClockViewModel();
                clockViewModel.DeleteFromListBox = DeleteFormListBox;
                clockViewModel.FetchData(model);
                clockView.DataContext = clockViewModel;
                item.Content = clockView;
                ListBoxItems.Add(item);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 添加需编辑的闹钟
        /// </summary>
        /// <param name="element"></param>
        /// <param name="model"></param>
        private void AddNewEditReminder(ListBox element, AlarmClockModel model)
        {
            try
            {
                ListBoxItem item = new ListBoxItem();
                //加载资源字典
                string packUri = @"/LifeService;component/WindowDictionary.xaml";
                ResourceDictionary myResourceDictionary = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
                item.SetValue(ListBoxItem.StyleProperty, myResourceDictionary["OnlyBackgroundListBoxItemStyle"]);

                AlarmClockEditView clockView = new AlarmClockEditView();
                AlarmClockViewModel clockViewModel = new AlarmClockViewModel();
                clockViewModel.DeleteFromListBox = DeleteFormListBox;
                clockViewModel.EditAlarmClock = EditAlarmClock;
                clockViewModel.FetchData(model);
                clockView.DataContext = clockViewModel;
                item.Content = clockView;
                ListBoxItems.Add(item);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 删除闹钟回调
        /// </summary>
        /// <param name="target"></param>
        private void DeleteFormListBox(ListBoxItem target)
        {
            try
            {
                ListBoxItems.Remove(target);
                if (target.Content is AlarmClockView)
                {
                    ((target.Content as AlarmClockView).DataContext as AlarmClockViewModel).DeleteData();
                }
                else if (target.Content is AlarmClockEditView)
                {
                    ((target.Content as AlarmClockEditView).DataContext as AlarmClockViewModel).DeleteData();
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 编辑闹钟
        /// </summary>
        /// <param name="model"></param>
        private void EditAlarmClock(AlarmClockModel model)
        {
            _temp = model;
            _lastUIElement = _mainBorder.Child;
            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            dLoginFadeOut.Completed += DLoginFadeIn_Completed;
            _mainBorder.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
        }

        /// <summary>
        /// 返回闹钟主页面回调
        /// </summary>
        /// <param name="target"></param>
        private void BackToMainView(string target)
        {
            try
            {
                ListBoxItems.Clear();
                foreach (AlarmClockModel model in MainAlarmClockManager.Instance.AlarmClocks)
                {
                    AddNewReminder(_mainListBox, model);
                }
                ButtonContent = "编辑";
                _mainBorder.Child = _lastUIElement;
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 外部关闭提醒回调
        /// </summary>
        private void CloseAlarmClock()
        {
            LifeServicePlugin.Instance.CloseLifeServiceAction -= CloseAlarmClock;
            _mainWindow.Close();
        }
    }
}
