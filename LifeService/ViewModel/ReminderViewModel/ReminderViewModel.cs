using System;
using System.Windows.Controls;
using System.Windows.Input;
using LifeService.Command;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using LifeService.Model.ReminderModel;
using System.Data;
using LifeService.Model;
using System.Windows;
using LifeService.View.ReminderView;
using System.Windows.Media.Animation;

namespace LifeService.ViewModel.ReminderViewModel
{
    internal class ReminderViewModel: ViewModelBase
    {
        private DeletgateCommand<Image> _mouseLeftButtonDownTitleCommand;
        private DeletgateCommand<Grid> _remoteRemindCommand;
        private DeletgateCommand<ListBox> _loadDataCommand;
        private DeletgateCommand<Border> _addButtonCommand;
        private DeletgateCommand<Button> _loadedAddButtonCommand;
        private DeletgateCommand<Border> _loadedBorderCommand;
        private DeletgateCommand<Window> _deactivatedCommand;
        private DeletgateCommand<Window> _windowDragCommand;
        private DeletgateCommand<Image> _loadedTitleContentCommand;
        private DeletgateCommand<Window> _loadedCommand;
        private ListBox _mainListBox;
        private Border _mainBorder;
        private Button _addButton;
        private UIElement _lastUIElement;
        private bool _isReminderRemote = false;
        private ReminderModel _temp;
        private Window _mainWindow;



        public ReminderViewModel()
        {
            _listBoxItems = new ObservableCollection<ListBoxItem>();
            TitleIcon = new BitmapImage(RemindMemoIconPath);
        }

        public ReminderViewModel(string reminderType)
        {
            TitleContent = reminderType;
            _listBoxItems = new ObservableCollection<ListBoxItem>();
            TitleIcon = new BitmapImage(RemindMemoIconPath);
        }

        #region 属性
        /// <summary>
        /// 提醒集合
        /// </summary>
        private ObservableCollection<ListBoxItem> _listBoxItems;
        public ObservableCollection<ListBoxItem> ListBoxItems
        {
            get { return _listBoxItems; }
            set
            {
                _listBoxItems = value;
                OnPropertyChanged("ListBoxItems");
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        private string _titleContent = "";
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
        /// 备忘提醒
        /// </summary>
        private Uri _remindMemoIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/ReminderImages/remind_memo.png", UriKind.RelativeOrAbsolute);
        public Uri RemindMemoIconPath
        {
            get { return _remindMemoIconPath; }
            set
            {
                _remindMemoIconPath = value;
                OnPropertyChanged("RemindMemoIconPath");
            }
        }

        /// <summary>
        /// 远程提醒
        /// </summary>
        private Uri _remindRemoteIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/ReminderImages/remind_remote.png", UriKind.RelativeOrAbsolute);
        public Uri RemindRemoteIconPath
        {
            get { return _remindRemoteIconPath; }
            set
            {
                _remindRemoteIconPath = value;
                OnPropertyChanged("RemindRemoteIconPath");
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        private ImageSource _titleIcon;

        public ImageSource TitleIcon
        {
            get { return _titleIcon; }
            set
            {
                _titleIcon = value;
                OnPropertyChanged("TitleIcon");
            }
        }
        #endregion

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

        public ICommand LoadedAddButtonCommand
        {
            get
            {
                if (_loadedAddButtonCommand == null)
                {
                    _loadedAddButtonCommand = new DeletgateCommand<Button>(LoadedAddButton, CanLoadedAddButton);
                }
                return _loadedAddButtonCommand;

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

        public ICommand MouseLeftButtonDownTitleCommand
        {
            get
            {
                if (_mouseLeftButtonDownTitleCommand == null)
                {
                    _mouseLeftButtonDownTitleCommand = new DeletgateCommand<Image>(MouseLeftButtonDownTitle, CanMouseLeftButtonDownTitle);
                }
                return _mouseLeftButtonDownTitleCommand;

            }
        }

        public ICommand LoadedTitleContentCommand
        {
            get
            {
                if (_loadedTitleContentCommand == null)
                {
                    _loadedTitleContentCommand = new DeletgateCommand<Image>(LoadedTitleContent, CanLoadedTitleContent);
                }
                return _loadedTitleContentCommand;

            }
        }

        public ICommand RemoteRemindCommand
        {
            get
            {
                if (_remoteRemindCommand == null)
                {
                    _remoteRemindCommand = new DeletgateCommand<Grid>(RemoteRemind, CanRemoteRemind);
                }
                return _remoteRemindCommand;

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
        #endregion

        /// <summary>
        /// 主窗口加载
        /// </summary>
        /// <param name="element"></param>
        private void Loaded(Window element)
        {
            _mainWindow = element;
            LifeServicePlugin.Instance.CloseLifeServiceAction += CloseReminder;
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
        /// 主border加载
        /// </summary>
        /// <param name="element"></param>
        private void LoadedAddButton(Button element)
        {
            _addButton = element;
        }

        /// <summary>
        /// 窗口移动
        /// </summary>
        /// <param name="element"></param>
        private void WindowDrag(Window element)
        {
            element.DragMove();
        }

        private void MouseLeftButtonDownTitle(Image element)
        {
            if (_isReminderRemote)
            {
                //提醒界面
                TitleIcon = new BitmapImage(RemindMemoIconPath);
                _isReminderRemote = false;
                _addButton.Visibility = Visibility.Visible;
            }
            else
            {
                //远程提醒
                TitleIcon = new BitmapImage(RemindRemoteIconPath);
                _isReminderRemote = true;
                _addButton.Visibility = Visibility.Hidden;
            }
            BackToMainView("");
        }

        private void LoadedTitleContent(Image element)
        {
            if (string.IsNullOrEmpty(TitleContent))
            {
                element.Visibility = Visibility.Visible;
            }
            else
            {
                element.Visibility = Visibility.Hidden;
                if (TitleContent.Equals("备忘提醒"))
                {

                }
                else if(TitleContent.Equals("远程提醒"))
                {

                }
            }
        }

        private void LoadData(ListBox element)
        {
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

        private void RemoteRemind(Grid element)
        {
            //MainRemoteReminderView view = new MainRemoteReminderView();
            //view.DataContext = new MainRemoteReminderViewModel();
            //element.Children.RemoveRange(0, Int32.MaxValue);
            //element.Children.Add(view);
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="element"></param>
        private void AddButton(Border element)
        {
            _mainBorder = element;
            _lastUIElement = element.Child;
            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            dLoginFadeOut.Completed += DLoginFadeOut_Completed;
            element.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
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

        #region 命令条件
        private static bool CanLoaded(Window element)
        {
            return true;
        }

        private static bool CanLoadedBorder(Border element)
        {
            return true;
        }

        private static bool CanLoadedAddButton(Button element)
        {
            return true;
        }

        private static bool CanLoadData(ListBox element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDownTitle(Image element)
        {
            return true;
        }

        private static bool CanLoadedTitleContent(Image element)
        {
            return true;
        }
        private static bool CanRemoteRemind(Grid element)
        {
            return true;
        }

        private static bool CanAddButton(Border element)
        {
            return true;
        }

        private static bool CanDeactivated(Window element)
        {
            return true;
        }

        private static bool CanWindowDrag(Window element)
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
            if (_isReminderRemote)
            {
                ReminderAddItemView view = new ReminderAddItemView();
                ReminderAddItemViewModel viewModel = new ReminderAddItemViewModel("RemoteReminder");
                viewModel.BackToMainView += BackToMainView;
                view.DataContext = viewModel;
                _mainBorder.Child = view;
            }
            else
            {
                ReminderAddItemView view = new ReminderAddItemView();
                ReminderAddItemViewModel viewModel = new ReminderAddItemViewModel("MemoReminder");
                viewModel.BackToMainView += BackToMainView;
                view.DataContext = viewModel;
                _mainBorder.Child = view;
            }
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <param name="element"></param>
        private void FetchData(ListBox element)
        {
            try
            {
                if (ListBoxItems.Count == 0)
                {
                    if (TitleContent.Equals("备忘提醒"))
                    {
                        foreach (var item in ReminderManager.Instance.Reminders.ToArray())
                        {
                            if (string.IsNullOrEmpty(item.Contact))
                            {
                                AddNewReminder(element, item);
                            }
                        }
                    }
                    else if(TitleContent.Equals("远程提醒"))
                    {
                        foreach (var item in ReminderManager.Instance.Reminders.ToArray())
                        {
                            if (!string.IsNullOrEmpty(item.Contact))
                            {
                                AddNewReminder(element, item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ReminderManager.Instance.Reminders.ToArray())
                        {
                            if (string.IsNullOrEmpty(item.Contact))
                            {
                                AddNewReminder(element, item);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }


        private void AddNewReminder(ContentControl element)
        {
            //int state = ReLocate((((element as ScrollViewer).Content as Viewbox).Child as Grid));
            //MemoReminderView reminderView = new MemoReminderView();
            //reminderView.DataContext = new MemoReminderViewModel();
            //Grid newGrid = new Grid
            //{
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Visibility = Visibility.Visible,
            //    Height = 100,
            //    Width = 320,
            //    Margin = new Thickness(0, 100* state, 0, 0),
            //};
            //newGrid.Children.Add(reminderView);
            //(((element as ScrollViewer).Content as Viewbox).Child as Grid).Children.Add(newGrid);
        }

        private void AddNewReminder(ListBox element, ReminderModel model)
        {
            try
            {
                ListBoxItem item = new ListBoxItem();
                //加载资源字典
                string packUri = @"/LifeService;component/WindowDictionary.xaml";
                ResourceDictionary myResourceDictionary = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
                item.SetValue(ListBoxItem.StyleProperty, myResourceDictionary["OnlyBackgroundListBoxItemStyle"]);

                MemoReminderView reminderView = new MemoReminderView();
                MemoReminderViewModel reminiderViewModel = new MemoReminderViewModel();
                reminiderViewModel.EditReminder = EditReminder;
                //clockViewModel.DeleteFromListBox = DeleteFormListBox;
                reminiderViewModel.FetchData(model);
                reminderView.DataContext = reminiderViewModel;
                item.Content = reminderView;
                ListBoxItems.Add(item);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 编辑提醒
        /// </summary>
        /// <param name="model"></param>
        private void EditReminder(ReminderModel model)
        {
            _temp = model;
            _lastUIElement = _mainBorder.Child;
            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            dLoginFadeOut.Completed += DLoginFadeIn_Completed;
            _mainBorder.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
        }

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLoginFadeIn_Completed(object sender, EventArgs e)
        {
            ReminderAddItemView view = new ReminderAddItemView();
            ReminderAddItemViewModel viewModel = new ReminderAddItemViewModel(_temp);
            viewModel.BackToMainView += BackToMainView;
            view.DataContext = viewModel;
            _mainBorder.Child = view;
        }

        /// <summary>
        /// 返回提醒主页面回调
        /// </summary>
        /// <param name="target"></param>
        private void BackToMainView(string target)
        {
            try
            {
                ListBoxItems.Clear();
                if (_isReminderRemote || TitleContent.Equals("远程提醒"))
                {
                    foreach (ReminderModel model in ReminderManager.Instance.Reminders.ToArray())
                    {
                        if (!string.IsNullOrEmpty(model.Contact))
                            AddNewReminder(_mainListBox, model);
                    }
                }
                else
                {
                    foreach (ReminderModel model in ReminderManager.Instance.Reminders.ToArray())
                    {
                        if (string.IsNullOrEmpty(model.Contact))
                            AddNewReminder(_mainListBox, model);
                    }
                }
                //ButtonContent = "编辑";
                //if (target.Equals("Cancel"))
                //{
                //    _mainBorder.Child = _lastUIElement;
                //}
                if (!string.IsNullOrEmpty(target))
                {
                    _mainBorder.Child = _lastUIElement;
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 外部关闭提醒回调
        /// </summary>
        private void CloseReminder()
        {
            LifeServicePlugin.Instance.CloseLifeServiceAction -= CloseReminder;
            _mainWindow.Close();
        }
    }
}
