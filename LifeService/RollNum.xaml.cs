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

namespace LifeService
{
    /// <summary>
    /// RollNum.xaml 的交互逻辑
    /// </summary>
    public partial class RollNum : UserControl
    {
        public RollNum()
        {
            InitializeComponent();
            
        }

        #region 属性

        #region YOffsetProperty 依赖属性

        public static readonly DependencyProperty YOffsetProperty = DependencyProperty.Register(
            "YOffset", typeof(double), typeof(RollNum),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(YOffsetPropertyChangedCallback)));

        private static void YOffsetPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            RollNum ucrollNum = sender as RollNum;
            ucrollNum.Layout();
        }

        private Point OldMousePoint = new Point();

        /// <summary>
        /// 获取或设置当前中间项序号
        /// </summary>
        public double YOffset
        {
            get
            {
                return (double)this.GetValue(YOffsetProperty);
            }
            set
            {
                if (value > PreHeight)
                {
                    this.SetValue(YOffsetProperty, 0.0);
                    CurrentIndex--;
                }
                else if (value < 0)
                {
                    this.SetValue(YOffsetProperty, PreHeight);
                    CurrentIndex++;
                }
                else
                {
                    this.SetValue(YOffsetProperty, value);
                }
            }
        }



        public double Yoffsetbak = 0.0;
        #endregion

        #region CurrentIndexProperty 依赖属性

        public static readonly DependencyProperty CurrentIndexProperty = DependencyProperty.Register(
            "CurrentIndex", typeof(int), typeof(RollNum),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(CurrentIndexPropertyChangedCallback)));

        private static void CurrentIndexPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            RollNum ucrollNum = sender as RollNum;
            ucrollNum.UpdateContent();
        }

        /// <summary>
        /// 获取或设置当前中间项序号
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return (int)this.GetValue(CurrentIndexProperty);
            }
            set
            {
                if (value < 0)
                {
                    this.SetValue(CurrentIndexProperty, Items.Count - 1);
                }
                else if (value >= Items.Count)
                {
                    this.SetValue(CurrentIndexProperty, 0);
                }
                else
                {
                    this.SetValue(CurrentIndexProperty, value);
                }

            }
        }

        #endregion

        private string currentitem;

        /// <summary>
        /// 当前的项目
        /// </summary>
        public string Currentitem
        {
            get
            {
                if (Items.Count > CurrentIndex)
                {
                    return Items[CurrentIndex].ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            set { }
        }

        private List<string> items = new List<string>();

        /// <summary>
        /// 所有的项目
        /// </summary>
        public List<string> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                FristLayOut();
                UpdateContent();
            }
        }

        private double sizescale = 0.5;

        /// <summary>
        /// 移动的距离
        /// </summary>
        public double SizeScale
        {
            get { return sizescale; }
            set
            {
                sizescale = value;
            }
        }

        private double opacitychange;

        /// <summary>
        /// 移动的距离
        /// </summary>
        public double OpacityChange
        {
            get { return opacitychange; }
            set
            {
                if ((value > 1) || (value < 0))
                {
                    OpacityChange = value;
                }
            }
        }

        /// <summary>
        /// 控件的高度
        /// </summary>
        private double controlheight = 260;

        /// <summary>
        /// 移动的距离
        /// </summary>
        public double ControlHeight
        {
            get { return controlheight; }
            set
            {
                controlheight = value;
            }
        }

        private double preheight;

        /// <summary>
        /// 每段的距离
        /// </summary>
        public double PreHeight
        {
            get
            {
                if (ControlHeight >= 0)
                {
                    if (preheight == 0)
                    {
                        preheight = ControlHeight / 5;
                    }
                    return preheight;
                }
                else
                {
                    if (this.ActualHeight >= 0)
                    {
                        return (this.ActualHeight / 5);
                    }
                    else
                    {
                        return 0;
                    }
                }

            }

        }

        Storyboard storyboard = new Storyboard();

        #endregion

        #region 方法

        private void FristLayOut()
        {
            YOffset = PreHeight / 2;
            Layout();
        }

        /// <summary>
        /// 布局
        /// </summary>
        private void Layout()
        {


            double Offset = 0;
            if (PreHeight != 0)
            {
                Offset = ((double)Label_Pre.GetValue(Canvas.TopProperty)) / PreHeight;
            }

            //透明度更新
            Label_Pre.Opacity = Offset * 0.5 + 0.5;
            Label_Current.Opacity = 1;
            Label_Next.Opacity = (1 - Offset * 0.5);

            Label_Pre.SetValue(Canvas.TopProperty, YOffset);
            Label_Current.SetValue(Canvas.TopProperty, YOffset + PreHeight);
            Label_Next.SetValue(Canvas.TopProperty, YOffset + PreHeight * 2);

            //Label_Pre.SetValue(Canvas.TopProperty, PreHeight / 2);
            //Label_Current.SetValue(Canvas.TopProperty, PreHeight / 2 + PreHeight);
            //Label_Next.SetValue(Canvas.TopProperty, PreHeight / 2 + PreHeight * 2);
        }

        /// <summary>
        /// 在动画结束后，自动回复到合适位置
        /// </summary>
        private void AutoFit()
        {
            double From = YOffset;
            if (double.IsNaN(From)) From = 0.0;
            double To = PreHeight / 2;

            MoveAnmition(From, To, this);
        }

        /// <summary>
        /// 在当前索引更新后，内容立即更新 
        /// 暂时没有想到更好的方法
        /// </summary>
        /// <param name="Type"></param>
        private void UpdateContent()
        {
            int count = Items.Count;
            if (count == 0)
            {
                Label_Pre.Content = "无数据";
                Label_Current.Content = "无数据";
                Label_Next.Content = "无数据";
            }
            else if (count == 1)
            {
                Label_Pre.Content = Items[0];
                Label_Current.Content = Items[0];
                Label_Next.Content = Items[0];
            }
            else if (count == 2)
            {
                if (CurrentIndex == 0)
                {
                    //Label_Pre.Content = Items[1];
                    Label_Current.Content = Items[0];
                    Label_Next.Content = Items[1];
                }
                else
                {
                    //Label_Pre.Content = Items[0];
                    Label_Current.Content = Items[1];
                    Label_Next.Content = Items[0];
                }
            }
            else if (count == 3)
            {
                if (CurrentIndex == 0)
                {
                    Label_Pre.Content = Items[2];
                    Label_Current.Content = Items[0];
                    Label_Next.Content = Items[1];
                }
                else if (CurrentIndex == 1)
                {
                    Label_Pre.Content = Items[0];
                    Label_Current.Content = Items[1];
                    Label_Next.Content = Items[2];
                }
                else
                {
                    Label_Pre.Content = Items[1];
                    Label_Current.Content = Items[2];
                    Label_Next.Content = Items[0];
                }
            }
            else if (count == 4)
            {
                if (CurrentIndex == 0)
                {
                    Label_Pre.Content = Items[3];
                    Label_Current.Content = Items[0];
                    Label_Next.Content = Items[1];
                }
                else if (CurrentIndex == 1)
                {
                    Label_Pre.Content = Items[0];
                    Label_Current.Content = Items[1];
                    Label_Next.Content = Items[2];
                }
                else if (CurrentIndex == 2)
                {
                    Label_Pre.Content = Items[1];
                    Label_Current.Content = Items[2];
                    Label_Next.Content = Items[3];
                }
                else
                {
                    Label_Pre.Content = Items[2];
                    Label_Current.Content = Items[3];
                    Label_Next.Content = Items[0];
                }
            }
            else
            {
                if ((CurrentIndex >= 0) && (CurrentIndex < 1))
                {
                    Label_Pre.Content = Items[count - 1];
                    Label_Current.Content = Items[CurrentIndex];
                    Label_Next.Content = Items[CurrentIndex + 1];
                }
                else if ((CurrentIndex >= 1) && (CurrentIndex < 2))
                {
                    Label_Pre.Content = Items[CurrentIndex - 1];
                    Label_Current.Content = Items[CurrentIndex];
                    Label_Next.Content = Items[CurrentIndex + 1];
                }
                else if ((CurrentIndex >= (count - 2)) && (CurrentIndex < (count - 1)))
                {
                    Label_Pre.Content = Items[CurrentIndex - 1];
                    Label_Current.Content = Items[CurrentIndex];
                    Label_Next.Content = Items[CurrentIndex + 1];
                }
                else if ((CurrentIndex >= (count - 1)) && (CurrentIndex < count))
                {
                    Label_Pre.Content = Items[CurrentIndex - 1];
                    Label_Current.Content = Items[CurrentIndex];
                    Label_Next.Content = Items[0];
                }
                else
                {
                    Label_Pre.Content = Items[CurrentIndex - 1];
                    Label_Current.Content = Items[CurrentIndex];
                    Label_Next.Content = Items[CurrentIndex + 1];
                }
            }

        }

        private void image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = touchPad;
            e.Mode = ManipulationModes.All;
        }


        private void image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var deltaManipulation = e.DeltaManipulation;

            YOffset += e.DeltaManipulation.Translation.Y * 0.5;
        }


        private void image_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            AutoFit();
        }

        private void touchPad_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // 移动惯性
            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 3 * 10.0 * 96.0 / (1000.0 * 1000.0)
            };

            e.Handled = true;
        }

        //回动动画
        private void MoveAnmition(double Fromvalue, double Tovalue, DependencyObject dobject)
        {

            DoubleAnimation xAnimation = new DoubleAnimation()
            {
                From = Fromvalue,
                To = Tovalue,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            };
            Storyboard.SetTarget(xAnimation, dobject);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("YOffset"));
            storyboard.Children.Add(xAnimation);
            storyboard.Completed += storyboard_Completed;
            storyboard.Begin(this, true);


        }

        private void storyboard_Completed(object sender, EventArgs e)
        {
            try
            {
                storyboard.Remove(this);
                YOffset = PreHeight / 2;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }


        }
        #endregion

        private void touchPad_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var deltaManipulation = e.Delta;

            var times = Math.Abs(e.Delta / 60);
            for (int i = 0; i < times; i++)
            {
                YOffset += e.Delta / times * 0.5;
            }
            AutoFit();
        }
    }
}
