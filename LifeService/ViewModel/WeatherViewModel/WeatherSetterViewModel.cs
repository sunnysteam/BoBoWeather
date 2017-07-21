using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using LifeService.View.WeatherView;
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

namespace LifeService.ViewModel.WeatherViewModel
{
    internal class WeatherSetterViewModel:ViewModelBase
    {
        private DeletgateCommand<ScrollViewer> _textChangedCommand;
        private DeletgateCommand<Grid> _backCommand;
        private DeletgateCommand<Grid> _isCheckedCommand;
        private DeletgateCommand<Grid> _loadedCheckedCommand;

        public Action<string,string> SearchResult;
        public Action BackToMainWeather;

        public WeatherSetterViewModel(string city, List<string> citynames, Degree degree)
        {
            City = city;
            CityNames = citynames;
            DegreeFormat = degree;
        }

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

        private List<string> _cityNames;
        public List<string> CityNames
        {
            get { return _cityNames; }
            set
            {
                _cityNames = value;
                OnPropertyChanged("CityNames");
            }
        }

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;
                OnPropertyChanged("CityName");
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

        public ICommand TextChangedCommand
        {
            get
            {
                if (_textChangedCommand == null)
                {
                    _textChangedCommand = new DeletgateCommand<ScrollViewer>(TextChanged, CanTextChanged);
                }
                return _textChangedCommand;

            }
        }

        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new DeletgateCommand<Grid>(Back, CanBack);
                }
                return _backCommand;

            }
        }

        public ICommand IsCheckedCommand
        {
            get
            {
                if (_isCheckedCommand == null)
                {
                    _isCheckedCommand = new DeletgateCommand<Grid>(IsChecked, CanIsChecked);
                }
                return _isCheckedCommand;

            }
        }

        public ICommand LoadedCheckedCommand
        {
            get
            {
                if (_loadedCheckedCommand == null)
                {
                    _loadedCheckedCommand = new DeletgateCommand<Grid>(LoadedChecked, CanLoadedChecked);
                }
                return _loadedCheckedCommand;

            }
        }

        private void LoadedChecked(Grid element)
        {
            foreach (var item in element.Children)
            {
                if (item is RadioButton)
                {
                    if ((item as RadioButton).Content.ToString().ToLower().Contains(DegreeFormat.ToString()))
                        (item as RadioButton).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                }
            }
        }

        /// <summary>
        /// 查询城市改变
        /// </summary>
        /// <param name="element"></param>
        private void TextChanged(ScrollViewer element)
        {
            try
            {
                if (!string.IsNullOrEmpty(CityName))
                {
                    (element.Content as Grid).Children.RemoveRange(0, int.MaxValue);

                    //模糊匹配城市
                    var result = from cityname in CityNames
                                 where cityname.Contains(CityName)
                                 select cityname;

                    if (result != null)
                    {
                        foreach (var cityname in result)
                        {
                            AddNewReminder(element, cityname);
                        }
                    }
                }
                else
                {
                    (element.Content as Grid).Children.RemoveRange(0, int.MaxValue);
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="element"></param>
        private void Back(Grid element)
        {
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(500);
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            DoubleAnimation dLoginFadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
                            dLoginFadeOut.Completed += DLoginFadeOut_Completed;
                            element.BeginAnimation(UIElement.OpacityProperty, dLoginFadeOut);
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
        /// 选择天气格式按钮
        /// </summary>
        /// <param name="element"></param>
        private void IsChecked(Grid element)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(200);
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            foreach (var item in element.Children)
                            {
                                if (item is RadioButton)
                                {
                                    if ((bool)(item as RadioButton).IsChecked)
                                    {
                                        (item as RadioButton).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                                    }
                                    else
                                    {
                                        (item as RadioButton).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#919eaa"));
                                    }
                                }
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

        private static bool CanTextChanged(ScrollViewer element)
        {
            return true;
        }

        private static bool CanBack(Grid element)
        {
            return true;
        }

        private static bool CanIsChecked(Grid element)
        {
            return true;
        }

        private static bool CanLoadedChecked(Grid element)
        {
            return true;
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLoginFadeOut_Completed(object sender, EventArgs e)
        {
            BackToMainWeather();
        }

        /// <summary>
        /// 新增模糊匹配的城市
        /// </summary>
        /// <param name="element"></param>
        private void AddNewReminder(ScrollViewer element,string targetName)
        {
            try
            {
                int state = (element.Content as Grid).Children.Count;
                WeatherDataView view = new WeatherDataView();
                WeatherDataViewModel viewmodel = new WeatherDataViewModel(CityName, targetName);
                viewmodel.SearchResult += SearchCity;
                view.DataContext = viewmodel;
                Grid newGrid = new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Visible,
                    Height = 42,
                    Width = 425,
                    Margin = new Thickness(0, 42 * state, 0, 0),
                };
                newGrid.Children.Add(view);
                (element.Content as Grid).Children.Add(newGrid);
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        /// <summary>
        /// 查询城市
        /// </summary>
        /// <param name="searchResult"></param>
        private void SearchCity(string searchResult)
        {
            SearchResult(searchResult, DegreeFormat.ToString());
        }

        /// <summary>
        /// 重新定位
        /// </summary>
        /// <param name="mainGrid"></param>
        /// <returns></returns>
        private int ReLocate(Grid mainGrid)
        {
            return mainGrid.Children.Count;
        }
    }
}
