using LifeService.Command;
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

namespace LifeService.ViewModel.AlarmClockViewModel
{
    internal class AlarmClockWeekSetViewModel : ViewModelBase
    {
        private DeletgateCommand<Grid> _mouseLeftButtonDownCommand;
        private DeletgateCommand<Grid> _clickBackCommand;
        private DeletgateCommand<Grid> _loadedCommand;
        public Action<List<WeekDay>> BackToMainView;

        public AlarmClockWeekSetViewModel(string days)
        {
            _weekList = new List<WeekDay>();
            FetchData(days);
        }

        private List<WeekDay> _weekList;
        public List<WeekDay> WeekList
        {
            get { return _weekList; }
            set { _weekList = value; }
        }

        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new DeletgateCommand<Grid>(Loaded, CanLoaded);
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

        private void Loaded(Grid element)
        {
            try
            {
                List<string> Days = (from day in WeekList
                                     select day.Day).ToList();

                foreach (var item in element.Children)
                {
                    if (Days.Contains(((item as Border).Child as Grid).Name))
                    {
                        foreach (var checkbox in ((item as Border).Child as Grid).Children)
                        {
                            if (checkbox is CheckBox)
                            {
                                (checkbox as CheckBox).IsChecked = true;
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

        private void MouseLeftButtonDown(Grid element)
        {
            try
            {
                SolidColorBrush myBrush = new SolidColorBrush();
                ColorAnimation myColorAnimation = new ColorAnimation((Color)ColorConverter.ConvertFromString("#153f5f"), Colors.Transparent, new Duration(TimeSpan.FromSeconds(0.2)));
                myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
                element.Background = myBrush;
                bool isChecked = false;
                foreach (var item in element.Children)
                {
                    if (item is CheckBox)
                    {
                        if (!(bool)(item as CheckBox).IsChecked)
                        {
                            (item as CheckBox).IsChecked = true;
                            isChecked = true;
                        }
                        else
                        {
                            (item as CheckBox).IsChecked = false;
                            isChecked = false;
                        }
                    }
                }
                foreach (var item in element.Children)
                {
                    if (item is Label)
                    {
                        if (isChecked)
                        {
                            AddWeekList((item as Label).Content.ToString());
                        }
                        else
                        {
                            RemoveWeekList((item as Label).Content.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
            }
        }

        private void ClickBack(Grid element)
        {
            WeekList = WeekList.OrderBy(temp => temp.Index).ToList();
            BackToMainView(WeekList);
        }

        private static bool CanClickBack(Grid element)
        {
            return true;
        }

        private static bool CanMouseLeftButtonDown(Grid element)
        {
            return true;
        }

        private static bool CanLoaded(Grid element)
        {
            return true;
        }

        private void AddWeekList(string weekday)
        {
            switch (weekday)
            {
                case "每周日":
                    WeekList.Add(new WeekDay("每周日", 7));
                    break;
                case "每周一":
                    WeekList.Add(new WeekDay("每周一", 1));
                    break;
                case "每周二":
                    WeekList.Add(new WeekDay("每周二", 2));
                    break;
                case "每周三":
                    WeekList.Add(new WeekDay("每周三", 3));
                    break;
                case "每周四":
                    WeekList.Add(new WeekDay("每周四", 4));
                    break;
                case "每周五":
                    WeekList.Add(new WeekDay("每周五", 5));
                    break;
                case "每周六":
                    WeekList.Add(new WeekDay("每周六", 6));
                    break;
            }
        }

        private void AddWeekList(int index)
        {
            switch (index)
            {
                case 7:
                    WeekList.Add(new WeekDay("每周日", 7));
                    break;
                case 1:
                    WeekList.Add(new WeekDay("每周一", 1));
                    break;
                case 2:
                    WeekList.Add(new WeekDay("每周二", 2));
                    break;
                case 3:
                    WeekList.Add(new WeekDay("每周三", 3));
                    break;
                case 4:
                    WeekList.Add(new WeekDay("每周四", 4));
                    break;
                case 5:
                    WeekList.Add(new WeekDay("每周五", 5));
                    break;
                case 6:
                    WeekList.Add(new WeekDay("每周六", 6));
                    break;
            }
        }

        private void RemoveWeekList(string weekday)
        {
            var result = WeekList.Where(temp => temp.Day == weekday).ToList();
            WeekList.Remove(result[0]);
        }

        private void FetchData(string days)
        {
            switch(days)
            {
                case "每天":
                    for (int i = 1; i <= 7; i++)
                    {
                        AddWeekList(i);
                    }
                    break;
                case "周末":
                    for (int i = 6; i <= 7; i++)
                    {
                        AddWeekList(i);
                    }
                    break;
                case "工作日":
                    for (int i = 1; i <= 5; i++)
                    {
                        AddWeekList(i);
                    }
                    break;
                default:
                    string[] _days = days.Split(' ');
                    for (int i = 0; i < _days.Length; i++)
                    {
                        AddWeekList("每" + _days[i]);
                    }
                    break;
            }
        }
    }

    internal class WeekDay
    {
        public WeekDay(string day,int index)
        {
            Day = day;
            Index = index;
        }

        public string Day { get; set; }
        public int Index { get; set; }
    }
}
