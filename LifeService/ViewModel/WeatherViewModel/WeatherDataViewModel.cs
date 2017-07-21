using GalaSoft.MvvmLight.Threading;
using LifeService.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LifeService.ViewModel.WeatherViewModel
{
    internal class WeatherDataViewModel:ViewModelBase
    {
        private DeletgateCommand<TextBlock> _loadedCommand;
        private DeletgateCommand<Grid> _mouseLeftButtonDownCommand;
        public Action<string> SearchResult;

        public WeatherDataViewModel(string matchWord, string targetWord)
        {
            MatchWord = matchWord;
            TargetWord = targetWord;
        }

        /// <summary>
        /// 匹配的字符
        /// </summary>
        private string _matchWord;
        public string MatchWord
        {
            get { return _matchWord; }
            set
            {
                _matchWord = value;
                OnPropertyChanged("MatchWord");
            }
        }

        /// <summary>
        /// 目标字符
        /// </summary>
        private string _targetWord;
        public string TargetWord
        {
            get { return _targetWord; }
            set
            {
                _targetWord = value;
                OnPropertyChanged("TargetWord");
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new DeletgateCommand<TextBlock>(Loaded, CanLoaded);
                }
                return _loadedCommand;

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

        private void Loaded(TextBlock element)
        {
            try
            {
                string s = TargetWord;
                element.Text = string.Empty;
                string split = MatchWord;
                while (s.IndexOf(split) >= 0)
                {
                    element.Inlines.Add(new Run(s.Substring(0, s.IndexOf(split))));
                    element.Inlines.Add(new Run(split) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0490ff")) });
                    s = s.Substring(s.IndexOf(split) + split.Length);
                }
                element.Inlines.Add(new Run(s));
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        private void MouseLeftButtonDown(Grid element)
        {
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            element.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#123652"));
                        });
                Thread.Sleep(200);
                SearchResult(TargetWord);

            }).ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    LogWriter.Instance.Error(result.Exception);
                }
            });
        }

        private static bool CanLoaded(TextBlock element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDown(Grid element)
        {
            return true;
        }
    }
}
