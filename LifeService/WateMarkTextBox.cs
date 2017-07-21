using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LifeService
{
    public class WateMarkTextBox : TextBox
    {
        private TextBlock wateMarkTextBlock;

        private TextBox wateMarkTextBox;

        static WateMarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WateMarkTextBox), new FrameworkPropertyMetadata(typeof(WateMarkTextBox)));
        }

        public WateMarkTextBox()
        {
            this.GotFocus += new RoutedEventHandler(WateMarkTextBox_GotFocus);

            this.LostFocus += new RoutedEventHandler(WateMarkTextBox_LostFocus);

            this.TextChanged += new TextChangedEventHandler(WateMarkTextBox_TextChanged);
        }

        void WateMarkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.wateMarkTextBox != null && !string.IsNullOrWhiteSpace(this.wateMarkTextBox.Text))
            {
                this.wateMarkTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                this.wateMarkTextBlock.Visibility = Visibility.Visible;
            }
        }

        void WateMarkTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.wateMarkTextBlock.Visibility = Visibility.Hidden;
            this.wateMarkTextBox.Focus();
        }

        void WateMarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.wateMarkTextBox != null && !string.IsNullOrWhiteSpace(this.wateMarkTextBox.Text) || !this.IsFocused)
            {
                this.wateMarkTextBlock.Visibility = Visibility.Hidden;
                this.Text = this.wateMarkTextBox.Text;
            }
            else
            {
                this.wateMarkTextBlock.Visibility = Visibility.Visible;
            }
        }


        public string WateMark
        {
            get { return (string)GetValue(WateMarkProperty); }

            set { SetValue(WateMarkProperty, value); }
        }

        public static DependencyProperty WateMarkProperty =
            DependencyProperty.Register("WateMark", typeof(string), typeof(WateMarkTextBox), new UIPropertyMetadata("水印"));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.wateMarkTextBlock = this.GetTemplateChild("ChildWateMark") as TextBlock;

            this.wateMarkTextBox = this.GetTemplateChild("ChildTextBox") as TextBox;
        }
    }
}
