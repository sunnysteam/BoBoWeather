using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LifeService.View.AlarmClockView
{
    /// <summary>
    /// AlarmClockView.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmClockView : UserControl
    {
        private ListBox _list;
        bool _isScroll = false;
        bool _isVertical = false;
        public AlarmClockView(ListBox list)
        {
            InitializeComponent();
            _list = list;
            //Decorator border = VisualTreeHelper.GetChild(list, 0) as Decorator;
            //if (border != null)
            //{
            //    (border.Child as ScrollViewer).AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(ScrollViewer_ScrollChanged1));
            //}
            list.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(ScrollViewer_ScrollChanged1));
        }

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private double _nowVerticalOffset = 0;
        private double _nowHorizontalOffset = 0;

        private void ScrollViewer_ScrollChanged_1(object sender, ScrollChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset - _nowVerticalOffset > 0 )
            {
                _isScroll = true;
                _isVertical = true;
                //if (_isFirstScroll)
                //{
                //    Decorator border = VisualTreeHelper.GetChild(_list, 0) as Decorator;
                //    if (border != null)
                //    {
                //        (border.Child as ScrollViewer).AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(ScrollViewer_ScrollChanged1));
                //    }
                //    _isFirstScroll = false;
                //}
                //(sender as ScrollViewer).ScrollToLeftEnd();
            }
            else if ((sender as ScrollViewer).VerticalOffset - _nowVerticalOffset == 0 )
            {
                //if (!_isFirstScroll)
                //{
                    _isScroll = true;
                    _isVertical = false;
                    //(sender as ScrollViewer).ScrollToLeftEnd();
                //}
                //else
                //{
                //    _isFirstScroll = false;
                //}
            }
            if ((sender as ScrollViewer).HorizontalOffset < 50)
            {
                (sender as ScrollViewer).ScrollToLeftEnd();
            }
            else if ((sender as ScrollViewer).HorizontalOffset > 50)
            {
                _isScroll = false;
                (sender as ScrollViewer).ScrollToRightEnd();
            }
            _nowHorizontalOffset = (sender as ScrollViewer).HorizontalOffset;
            _nowVerticalOffset = (sender as ScrollViewer).VerticalOffset;
            (sender as ScrollViewer).ScrollToTop();
        }

        private void ScrollViewer_ScrollChanged1(object sender, RoutedEventArgs e)
        {
            if (_isScroll)
            {
                if (_isVertical)
                {
                    //_list.ScrollIntoView(_list.SelectedIndex);
                    //_list.ScrollIntoView(_list.Items[_list.Items.Count - 1]);
                    //_list.SelectedIndex = _list.SelectedIndex + 6;
                    Decorator border = VisualTreeHelper.GetChild(sender as ListBox, 0) as Decorator;
                    if (border != null)
                    {
                        //ScrollViewer scrollViewer = (border.Child as ScrollViewer);
                        //if (scrollViewer != null)
                        //{
                        (border.Child as ScrollViewer).ScrollToVerticalOffset((border.Child as ScrollViewer).VerticalOffset + 10);
                        //}
                    }
                    ////DoubleAnimation dLoginFadeOut = new DoubleAnimation(_Scroll.VerticalOffset, _Scroll.VerticalOffset + 60, new Duration(TimeSpan.FromSeconds(0.2)));
                    //_Scroll.BeginAnimation(TranslateTransform.YProperty, dLoginFadeOut);

                    //(sender as ScrollViewer).ScrollToVerticalOffset((sender as ScrollViewer).VerticalOffset + 80);
                    _isScroll = false;
                }
                else
                {
                    //_list.ScrollIntoView(_list.SelectedIndex);
                    //_list.SelectedIndex = _list.SelectedIndex - 6;
                    //Decorator border = VisualTreeHelper.GetChild(_list, 0) as Decorator;
                    //if (border != null)
                    //{
                    //    ScrollViewer scrollViewer = (border.Child as ScrollViewer);
                    //    if (scrollViewer != null)
                    //    {
                    //        (border.Child as ScrollViewer).ScrollToVerticalOffset((border.Child as ScrollViewer).VerticalOffset - 80);
                    //    }
                    //}
                    Decorator border = VisualTreeHelper.GetChild(sender as ListBox, 0) as Decorator;
                    if (border != null)
                    {
                        //ScrollViewer scrollViewer = border.Child as ScrollViewer;
                        //if (scrollViewer != null)
                        //{
                        (border.Child as ScrollViewer).ScrollToVerticalOffset((border.Child as ScrollViewer).VerticalOffset - 5);
                        //}
                    }
                    //DoubleAnimation dLoginFadeOut = new DoubleAnimation(_Scroll.VerticalOffset, _Scroll.VerticalOffset - 60, new Duration(TimeSpan.FromSeconds(0.2)));
                    //_Scroll.BeginAnimation(TranslateTransform.YProperty, dLoginFadeOut);
                    //(sender as ScrollViewer).ScrollToVerticalOffset((sender as ScrollViewer).VerticalOffset - 80);
                    _isScroll = false;
                }
            }
        }

        //private void mainGrid_PreviewTouchMove(object sender, TouchEventArgs e)
        //{
        //}

        //private void mainGrid_TouchMove(object sender, RoutedEventArgs e)
        //{

        //    //(e as TouchEventArgs).Handled = true;
        //    ListBox _canvas = (ListBox)sender as ListBox;
        //    //if (_canvas != null && (e as TouchEventArgs).TouchDevice.Captured == _canvas)
        //    //{
        //    TouchPoint tp = (e as TouchEventArgs).GetTouchPoint(_canvas);
        //    if ((e as TouchEventArgs).TouchDevice.Id == firstTouchId)
        //    {
        //        if (!_isVertical)
        //        {
        //            mainBorder.Margin = new Thickness(-52, 0, 0, 0);
        //            _isVertical = true;
        //        }
        //        else
        //        {
        //            mainBorder.Margin = new Thickness(28, 0, -80, 0);
        //            _isVertical = false;
        //        }

        //    }


        //    //}

        //}
        //private void mainGrid_PreviewTouchDown(object sender, TouchEventArgs e)
        //{
        //    Grid _canvas = (Grid)sender as Grid;
        //    //TouchPoint tp = e.GetTouchPoint(_canvas);
        //    //pt1.X = tp.Position.X;
        //    //pt1.Y = tp.Position.Y;
        //    if (_canvas != null)
        //    {
        //        //_canvas.Children.Clear();
        //        e.TouchDevice.Capture(_canvas);

        //        // Record the ID of the first touch point if it hasn't been recorded.
        //        if (firstTouchId == -1)
        //        {
        //            TouchPoint tp = e.GetTouchPoint(_canvas);
        //            pt1.X = tp.Position.X;
        //            pt1.Y = tp.Position.Y;
        //            firstTouchId = e.TouchDevice.Id;
        //        }
        //    }

        //}
    }
}
