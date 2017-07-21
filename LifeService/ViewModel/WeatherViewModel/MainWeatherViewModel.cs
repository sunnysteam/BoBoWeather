using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.Model;
using LifeService.Model.Weathermodel;
using LifeService.Model.WeatherModel;
using LifeService.View.WeatherView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace LifeService.ViewModel.WeatherViewModel
{
    internal class MainWeatherViewModel:ViewModelBase
    {
        private DeletgateCommand<Window> _windowDragCommand;
        private DeletgateCommand<Window> _deactivatedCommand;
        private DeletgateCommand<Window> _loadedCommand;
        private DeletgateCommand<Border> _setterCommand;
        private Border _mainBorder;
        private Window _thisWindow;
        private UIElement _lastUIElement;
        public Action<string,string> SearchResult;

        public MainWeatherViewModel(WeatherDataModel nowModel, WeatherDataModel dailyModel, Degree degreeFormat)
        {
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            DailyModel = dailyModel;
                            NowModel = nowModel;
                            DegreeFormat = degreeFormat;
                            FetchData();
                        });
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    //result.Exception.GetBaseException()
                }
            });
        }

        private Degree _degreeFormat;
        public Degree DegreeFormat
        {
            get { return _degreeFormat; }
            set { _degreeFormat = value; }
        }

        private WeatherDataModel _nowModel;
        public WeatherDataModel NowModel
        {
            get { return _nowModel; }
            set { _nowModel = value; }
        }


        private WeatherDataModel _dailyModel;
        public WeatherDataModel DailyModel
        {
            get { return _dailyModel; }
            set { _dailyModel = value; }
        }

        #region 当前
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        private string _nowWeather;
        public string NowWeather
        {
            get { return _nowWeather; }
            set
            {
                _nowWeather = value;
                OnPropertyChanged("NowWeather");
            }
        }

        private string _nowDegree;
        public string NowDegree
        {
            get { return _nowDegree; }
            set
            {
                _nowDegree = value + "°";
                OnPropertyChanged("NowDegree");
            }
        }

        private Uri _nowIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherBigImages/99.png", UriKind.RelativeOrAbsolute);
        public Uri NowIconPath
        {
            get { return _nowIconPath; }
            set
            {
                _nowIconPath = value;
                OnPropertyChanged("NowIconPath");
            }
        }

        private ImageSource _nowWeatherIcon;
        public ImageSource NowWeatherIcon
        {
            get { return _nowWeatherIcon; }
            set
            {
                _nowWeatherIcon = value;
                OnPropertyChanged("NowWeatherIcon");
            }
        }
        #endregion

        #region 今天
        private string _firstWeather;
        public string FirstWeather
        {
            get { return _firstWeather; }
            set
            {
                _firstWeather = value;
                OnPropertyChanged("FirstWeather");
            }
        }

        private string _firstDegree;
        public string FirstDegree
        {
            get { return _firstDegree; }
            set
            {
                _firstDegree = value;
                OnPropertyChanged("FirstDegree");
            }
        }

        private Uri _firstIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherSmallImages/99.png", UriKind.RelativeOrAbsolute);
        public Uri FirstIconPath
        {
            get { return _firstIconPath; }
            set
            {
                _firstIconPath = value;
                OnPropertyChanged("FirstIconPath");
            }
        }

        private ImageSource _firstWeatherIcon;
        public ImageSource FirstWeatherIcon
        {
            get { return _firstWeatherIcon; }
            set
            {
                _firstWeatherIcon = value;
                OnPropertyChanged("FirstWeatherIcon");
            }
        }
        #endregion

        #region 明天
        private string _secondWeather;
        public string SecondWeather
        {
            get { return _secondWeather; }
            set
            {
                _secondWeather = value;
                OnPropertyChanged("SecondWeather");
            }
        }

        private string _secondDegree;
        public string SecondDegree
        {
            get { return _secondDegree; }
            set
            {
                _secondDegree = value;
                OnPropertyChanged("SecondDegree");
            }
        }

        private Uri _secondIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherSmallImages/99.png", UriKind.RelativeOrAbsolute);
        public Uri SecondIconPath
        {
            get { return _secondIconPath; }
            set
            {
                _secondIconPath = value;
                OnPropertyChanged("SecondIconPath");
            }
        }

        private ImageSource _secondWeatherIcon;
        public ImageSource SecondWeatherIcon
        {
            get { return _secondWeatherIcon; }
            set
            {
                _secondWeatherIcon = value;
                OnPropertyChanged("SecondWeatherIcon");
            }
        }
        #endregion

        #region 后天
        private string _thirdWeather;
        public string ThirdWeather
        {
            get { return _thirdWeather; }
            set
            {
                _thirdWeather = value;
                OnPropertyChanged("ThirdWeather");
            }
        }

        private string _thirdDegree;
        public string ThirdDegree
        {
            get { return _thirdDegree; }
            set
            {
                _thirdDegree = value;
                OnPropertyChanged("ThirdDegree");
            }
        }

        private Uri _thirdIconPath = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherSmallImages/99.png", UriKind.RelativeOrAbsolute);
        public Uri ThirdIconPath
        {
            get { return _thirdIconPath; }
            set
            {
                _thirdIconPath = value;
                OnPropertyChanged("ThirdIconPath");
            }
        }

        private ImageSource _thirdWeatherIcon;
        public ImageSource ThirdWeatherIcon
        {
            get { return _thirdWeatherIcon; }
            set
            {
                _thirdWeatherIcon = value;
                OnPropertyChanged("ThirdWeatherIcon");
            }
        }
        #endregion

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

        public ICommand SetterCommand
        {
            get
            {
                if (_setterCommand == null)
                {
                    _setterCommand = new DeletgateCommand<Border>(Setter, CanSetter);
                }
                return _setterCommand;

            }
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

        private void Loaded(Window element)
        {
            _thisWindow = element;
            LifeServicePlugin.Instance.CloseLifeServiceAction += CloseWeather;
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="element"></param>
        private void Setter(Border element)
        {
            _mainBorder = element;
            _lastUIElement = element.Child;
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(500);//让动画完成
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
                            dLoginFadeOut.Completed += DLoginFadeOut_Completed;
                            element.Child.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
                        });
            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                }
            });
        }

        private static bool CanWindowDrag(Window element)
        {
            return true;
        }

        private static bool CanDeactivated(Window element)
        {
            return true;
        }

        private static bool CanLoaded(Window element)
        {
            return true;
        }

        private static bool CanSetter(Border element)
        {
            return true;
        }

        /// <summary>
        /// 动画完成后加载设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLoginFadeOut_Completed(object sender, EventArgs e)
        {
            try
            {
                DataTable table = DataManager.Instance.SelectTables(new CityModel());
                List<string> cityNames = new List<string>();
                foreach (DataRow item in table.Rows)
                {
                    cityNames.Add(item[0].ToString());
                }
                WeatherSetterView view = new WeatherSetterView();
                WeatherSetterViewModel viewModel = new WeatherSetterViewModel(City, cityNames, DegreeFormat);
                viewModel.SearchResult += SearchCity;
                viewModel.BackToMainWeather += BackFromSetter;
                view.DataContext = viewModel;
                _mainBorder.Child = view;
            }
            catch(Exception ex)
            {
                LogWriter.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 查询城市的回调
        /// </summary>
        /// <param name="searchResult"></param>
        /// <param name="degreeFormat"></param>
        private void SearchCity(string searchResult,string degreeFormat)
        {
            SearchResult(searchResult, degreeFormat);
            DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
                            dLoginFadeOut.Completed += DLoginFadeOut_Completed1;
                            _thisWindow.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
                        });
        }

        private void DLoginFadeOut_Completed1(object sender, EventArgs e)
        {
            _thisWindow.Hide();
        }

        /// <summary>
        /// 返回按钮回调
        /// </summary>
        private void BackFromSetter()
        {
            _mainBorder.Child = _lastUIElement;
        }

        private void FetchData()
        {
            try
            {
                if (NowModel != null)
                {
                    City = NowModel.Results[0].Location.Name;
                    NowDegree = NowModel.Results[0].Now.Temperature;
                    NowWeather = NowModel.Results[0].Now.Text;
                    NowIconPath =
                        new Uri(
                            String.Format(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherBigImages/{0}.png",
                                NowModel.Results[0].Now.Code), UriKind.RelativeOrAbsolute);
                    NowWeatherIcon = new BitmapImage(NowIconPath);
                }
                if (DailyModel != null)
                {
                    FirstDegree = DailyModel.Results[0].Daily[0].Low + "°/" + DailyModel.Results[0].Daily[0].High + "°";
                    FirstWeather = DailyModel.Results[0].Daily[0].Text_Day;
                    FirstIconPath =
                        new Uri(
                            String.Format(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherSmallImages/{0}.png",
                                DailyModel.Results[0].Daily[0].Code_Day), UriKind.RelativeOrAbsolute);
                    FirstWeatherIcon = new BitmapImage(FirstIconPath);

                    SecondDegree = DailyModel.Results[0].Daily[1].Low + "°/" + DailyModel.Results[0].Daily[1].High + "°";
                    SecondWeather = DailyModel.Results[0].Daily[1].Text_Day;
                    SecondIconPath =
                        new Uri(
                            String.Format(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherSmallImages/{0}.png",
                                DailyModel.Results[0].Daily[1].Code_Day), UriKind.RelativeOrAbsolute);
                    SecondWeatherIcon = new BitmapImage(SecondIconPath);

                    ThirdDegree = DailyModel.Results[0].Daily[2].Low + "°/" + DailyModel.Results[0].Daily[2].High + "°";
                    ThirdWeather = DailyModel.Results[0].Daily[2].Text_Day;
                    ThirdIconPath =
                        new Uri(
                            String.Format(AppDomain.CurrentDomain.BaseDirectory + "/Resources/WeatherSmallImages/{0}.png",
                                DailyModel.Results[0].Daily[2].Code_Day), UriKind.RelativeOrAbsolute);
                    ThirdWeatherIcon = new BitmapImage(ThirdIconPath);
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
        private void CloseWeather()
        {
            LifeServicePlugin.Instance.CloseLifeServiceAction -= CloseWeather;
            _thisWindow.Close();
        }
    }
}
