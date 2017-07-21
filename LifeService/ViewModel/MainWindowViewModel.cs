using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifeService.Command;
using LifeService.Model;
using LifeService.View.AlarmClockView;
using LifeService.View.WeatherView;
using LifeService.ViewModel.AlarmClockViewModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Controls.Primitives;
using System.Threading;
using LifeService.ViewModel.WeatherViewModel;
using System.Windows.Media;
using LifeService.Model.Weathermodel;
using LifeService.Model.WeatherModel;
using System.Data;
using System.Windows.Media.Imaging;
using LifeService.View.ReminderView;
using System.IO;
using System.Collections.Generic;
using LifeService.ViewModel.ReminderViewModel;
using Newtonsoft.Json;

namespace LifeService.ViewModel
{
    internal class MainWindowViewModel: ViewModelBase
    {
        #region 命令对象
        private DeletgateCommand<ButtonBase> reminderCommand;
        private DeletgateCommand<ButtonBase> weatherCommand;
        private DeletgateCommand<ButtonBase> mainAlarmClockCommand;
        private DeletgateCommand<Window> mainWindowMouseDownCommand;
        private DeletgateCommand<Grid> backButtonCommand;
        private DeletgateCommand<Grid> viewBoxCommand;
        private DeletgateCommand<Window> mainWindowCloseCommand;
        private DeletgateCommand<ScrollViewer> weatherScrollViewCommand;
        private DeletgateCommand<ScrollViewer> cityScrollViewCommand;
        #endregion

        private Decorator view_Box;
        private Timer _getWeatherTimer;
        public MainWindowViewModel()
        {
            //每小时更新天气
            _getWeatherTimer = new Timer(new TimerCallback(callback =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                       () =>
                       {
                           GetWeatherData();
                       });
            }), null, 1000 * 60 * 60, 1000 * 60 * 60);
        }

        #region 天气属性
        private string _key = "uvqkmy75frap92cs";

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                string[] temp = value.Split('市');
                temp = temp[0].Split('特');
                _city = temp[0];
                OnPropertyChanged("City");
            }
        }

        private Degree _degreeFormat;
        public Degree DegreeFormat
        {
            get { return _degreeFormat; }
            set
            {
                _degreeFormat = value;
                OnPropertyChanged("DegreeFormat");
            }
        }

        private Uri _iconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Images/99.png", UriKind.RelativeOrAbsolute);
        public Uri IconPath
        {
            get { return _iconPath; }
            set
            {
                _iconPath = value;
                OnPropertyChanged("IconPath");
            }
        }

        private ImageSource _weatherIcon;
        public ImageSource WeatherIcon
        {
            get { return _weatherIcon; }
            set
            {
                _weatherIcon = value;
                OnPropertyChanged("WeatherIcon");
            }
        }

        private WeatherDataModel _now = new WeatherDataModel();
        public WeatherDataModel Now
        {
            get { return _now; }
            set
            {
                _now = value;
                OnPropertyChanged("Now");
            }
        }

        private WeatherDataModel _dailyModel;
        public WeatherDataModel DailyModel
        {
            get { return _dailyModel; }
            set
            {
                _dailyModel = value;
                OnPropertyChanged("DailyModel");
            }
        }

        private string _resultDegree;
        public string ResultDegree
        {
            get { return _resultDegree; }
            set
            {
                _resultDegree = value + "°";
                OnPropertyChanged("ResultDegree");
            }
        }

        private string _resultCity;
        public string ResultCity
        {
            get { return _resultCity; }
            set
            {
                _resultCity = value;
                OnPropertyChanged("ResultCity");
            }
        }

        private string _resultWeather;
        public string ResultWeather
        {
            get { return _resultWeather; }
            set
            {
                _resultWeather = value;
                OnPropertyChanged("ResultWeather");
            }
        }
        #endregion


        #region 命令属性
        public ICommand ViewBoxCommand
        {
            get
            {
                if (viewBoxCommand == null)
                {
                    viewBoxCommand = new DeletgateCommand<Grid>(ViewBox, CanViewBoxCommand);
                }
                return viewBoxCommand;

            }
        }

        public ICommand WeatherScrollViewCommand
        {
            get
            {
                if (weatherScrollViewCommand == null)
                {
                    weatherScrollViewCommand = new DeletgateCommand<ScrollViewer>(WeatherScrollView, CanWeatherScrollViewCommand);
                }
                return weatherScrollViewCommand;

            }
        }

        public ICommand CityScrollViewCommand
        {
            get
            {
                if (cityScrollViewCommand == null)
                {
                    cityScrollViewCommand = new DeletgateCommand<ScrollViewer>(CityScrollView, CanCityScrollViewCommand);
                }
                return cityScrollViewCommand;

            }
        }

        public ICommand RemindCommand
        {
            get
            {
                if (reminderCommand == null)
                {
                    reminderCommand = new DeletgateCommand<ButtonBase>(Remind, CanRemindCommand);
                }
                return reminderCommand;

            }
        }

        public ICommand WeatherCommand
        {
            get
            {
                if (weatherCommand == null)
                {
                    weatherCommand = new DeletgateCommand<ButtonBase>(Weather, CanWeatherCommand);
                }
                return weatherCommand;

            }
        }

        public ICommand MainAlarmClockCommand
        {
            get
            {
                if (mainAlarmClockCommand == null)
                {
                    mainAlarmClockCommand = new DeletgateCommand<ButtonBase>(MainAlarmClock, CanMainAlarmClockCommand);
                }
                return mainAlarmClockCommand;

            }
        }

        public ICommand MainWindowMouseDownCommand
        {
            get
            {
                if (mainWindowMouseDownCommand == null)
                {
                    mainWindowMouseDownCommand = new DeletgateCommand<Window>(MainWindowMouseDown, CanMainWindowMouseDownCommand);
                }
                return mainWindowMouseDownCommand;

            }
        }

        public ICommand BackButtonCommand
        {
            get
            {
                if (backButtonCommand == null)
                {
                    backButtonCommand = new DeletgateCommand<Grid>(BackButton, CanBackButtonCommand);
                }
                return backButtonCommand;

            }
        }

        public ICommand MainWindowCloseCommand
        {
            get
            {
                if (mainWindowCloseCommand == null)
                {
                    mainWindowCloseCommand = new DeletgateCommand<Window>(MainWindowClose, CanMainWindowCloseCommand);
                }
                return mainWindowCloseCommand;

            }
        }
        #endregion

        #region 命令逻辑
        /// <summary>
        /// 加载主界面
        /// </summary>
        /// <param name="element"></param>
        private void ViewBox(Grid element)
        {
            try
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                       () =>
                       {
                           //获取历史天气
                           WeatherDataModel[] objects = WeatherManager.Instance.ReadWeatherHistory();
                           
                           if (objects != null)
                           {
                               Now = objects[0];
                               DailyModel = objects[1];
                               if (Now != null)
                               {
                                   ResultCity = Now.Results[0].Location.Name;
                                   ResultDegree = Now.Results[0].Now.Temperature;
                                   ResultWeather = Now.Results[0].Now.Text;
                                   IconPath =
                                       new Uri(
                                           String.Format(AppDomain.CurrentDomain.BaseDirectory + "/Resources/MainWeatherImages/{0}.png",
                                               Now.Results[0].Now.Code), UriKind.RelativeOrAbsolute);
                                   WeatherIcon = new BitmapImage(IconPath);
                               }
                           }
                           else
                           {
                               IconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/MainWeatherImages/99.png", UriKind.RelativeOrAbsolute);
                               WeatherIcon = new BitmapImage(IconPath);
                           }
                       });

                view_Box = element.Children[0] as Decorator;
                WeatherModel model = new WeatherModel();
                DataTable table = DataManager.Instance.SelectTables(model);
                if (table != null && table.Rows.Count > 0)
                {
                    model.FetchData(table.Rows[0]);
                    FetchData(model);
                    GetWeatherData();
                }
                else
                {
                    WeatherIcon = new BitmapImage(IconPath);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                LogWriter.Instance.Error(e);
            }
        }


        /// <summary>
        /// 调取天气方法
        /// </summary>
        private void GetWeatherData()
        {
            Task.Factory.StartNew(() =>
            {
                string now = "https://api.thinkpage.cn/v3/weather/now.json?key={0}&location={1}&language=zh-Hans&unit={2}";//实时天气
                string daily = "https://api.thinkpage.cn/v3/weather/daily.json?key={0}&location={1}&language=zh-Hans&unit={2}&start=0&days=5";//历史天气
                now = String.Format(now, Key, City, DegreeFormat.ToString());
                daily = String.Format(daily, Key, City, DegreeFormat.ToString());
                Now = WeatherManager.Instance.GetWeatherDataNow(now, City);
                DailyModel = WeatherManager.Instance.GetWeatherDataDaily(daily, City);
                WeatherManager.Instance.WriteWeatherHistory(Now, DailyModel);
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            if (Now != null)
                            {
                                ResultCity = Now.Results[0].Location.Name;
                                ResultDegree = Now.Results[0].Now.Temperature;
                                ResultWeather = Now.Results[0].Now.Text;
                                IconPath =
                                    new Uri(
                                        String.Format(AppDomain.CurrentDomain.BaseDirectory + "/Resources/MainWeatherImages/{0}.png",
                                            Now.Results[0].Now.Code), UriKind.RelativeOrAbsolute);
                                WeatherIcon = new BitmapImage(IconPath);
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
        /// 提醒按钮
        /// </summary>
        /// <param name="element"></param>
        private void Remind(ButtonBase element)
        {
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(500);
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            ReminderView view = new ReminderView();
                            view.DataContext = new ReminderViewModel.ReminderViewModel();
                            view.Show();
                        });
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                }
            });
        }

        /// <summary>
        /// 天气按钮
        /// </summary>
        /// <param name="element"></param>
        private void Weather(ButtonBase element)
        {
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(500);
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            MainWeatherView view = new MainWeatherView();
                            MainWeatherViewModel viewModel = new MainWeatherViewModel(Now, DailyModel, DegreeFormat);
                            viewModel.SearchResult += SearchCity;
                            view.DataContext = viewModel;
                            view.Show();
                        });
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                }
            });
        }

        /// <summary>
        /// 闹钟按钮
        /// </summary>
        /// <param name="element"></param>
        private void MainAlarmClock(ButtonBase element)
        {
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(500);
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            MainAlarmClockView view = new MainAlarmClockView();
                            view.DataContext = new MainAlarmClockViewModel();
                            view.Show();
                        });
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                }
            });
        }

        #region 天气滚动
        /// <summary>
        /// 字幕滚动
        /// </summary>
        private Timer _mainWeatherTimer = null;
        private void WeatherScrollView(ScrollViewer element)
        {
            if (element != null)
            {
                _mainWeatherTimer = new Timer(new TimerCallback(maindcallback =>
                {
                    try
                    {
                        Timer _weatherTimer = null;
                        if (_weatherTimer == null)
                        {
                            _weatherTimer = new Timer(new TimerCallback(callback =>
                            {

                                DispatcherHelper.CheckBeginInvokeOnUI(
                                       () =>
                                       {
                                           element.ScrollToHorizontalOffset(element.HorizontalOffset + 0.5);
                                       });
                            }), null, 0, 50);
                        }
                        if (element.HorizontalOffset >= element.ExtentWidth - element.ActualWidth)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(
                                () =>
                                {
                                    element.ScrollToHorizontalOffset(0);

                                });
                            _weatherTimer?.Dispose();
                            _weatherTimer = null;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }), null, 5000, 4000);
            }
        }

        private Timer _mainCityTimer = null;
        private void CityScrollView(ScrollViewer element)
        {
            if (element != null)
            {
                _mainCityTimer = new Timer(new TimerCallback(maindcallback =>
                {
                    try
                    {
                        Timer _cityTimer = null;
                        if (_cityTimer == null)
                        {
                            _cityTimer = new Timer(new TimerCallback(callback =>
                            {

                                DispatcherHelper.CheckBeginInvokeOnUI(
                                       () =>
                                       {
                                           element.ScrollToHorizontalOffset(element.HorizontalOffset + 0.5);
                                       });
                            }), null, 0, 50);
                        }
                        if (element.HorizontalOffset >= element.ExtentWidth - element.ActualWidth)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(
                                () =>
                                {
                                    element.ScrollToHorizontalOffset(0);

                                });
                            _cityTimer?.Dispose();
                            _cityTimer = null;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }), null, 5000, 4000);
            }
        }
        #endregion

        private void MainWindowMouseDown(Window element)
        {
            element.DragMove();
        }

        private void MainWindowClose(Window element)
        {
            element.Hide();
        }

        private void BackButton(Grid element)
        {
            element.Children.RemoveRange(0, Int32.MaxValue);
            element.Children.Add(view_Box);
        }
        #endregion

        #region 命令条件
        private static bool CanViewBoxCommand(Panel element)
        {
            return true;
        }
        private static bool CanWeatherScrollViewCommand(ScrollViewer element)
        {
            return true;
        }

        private static bool CanCityScrollViewCommand(ScrollViewer element)
        {
            return true;
        }

        private static bool CanRemindCommand(ButtonBase element)
        {
            return true;
        }

        private static bool CanWeatherCommand(ButtonBase element)
        {
            return true;
        }

        private static bool CanMainAlarmClockCommand(ButtonBase element)
        {
            return true;
        }

        private static bool CanMainWindowMouseDownCommand(ContentControl element)
        {
            return true;
        }

        private static bool CanMainWindowCloseCommand(ContentControl element)
        {
            return true;
        }

        private static bool CanBackButtonCommand(Panel element)
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 回调需要查询的城市
        /// </summary>
        /// <param name="searchResult"></param>
        /// <param name="degreeFormat"></param>
        private void SearchCity(string searchResult, string degreeFormat)
        {
            City = searchResult;
            DegreeFormat = (Degree)Enum.Parse(typeof(Degree), degreeFormat);
            GetWeatherData();
            WeatherModel model = new WeatherModel();
            model.City = City;
            model.DegreeFormat = DegreeFormat.ToString();
            DataManager.Instance.UpdateTables(model);
        }

        /// <summary>
        /// 取数据
        /// </summary>
        /// <param name="model"></param>
        public void FetchData(WeatherModel model)
        {
            City = model.City;
            DegreeFormat = (Degree)Enum.Parse(typeof(Degree), model.DegreeFormat);
        }
    }
}
