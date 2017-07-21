using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Drawing = System.Drawing;
using System.Windows.Forms;

namespace DesktopWeather
{
    public class GWindow : Window
    {
        //托盘
        NotifyIcon notifyIcon;
        //注册AreaIcon属性，用于托盘的图标
        public static readonly DependencyProperty AreaIconProperty =
            DependencyProperty.Register("AreaIcon", typeof(ImageSource), typeof(GWindow));
        //注册AreaText属性，用于鼠标滑到托盘图标时显示的文字
        public static readonly DependencyProperty AreaTextProperty =
            DependencyProperty.Register("AreaText", typeof(string), typeof(GWindow));
        //注册AreaVisibility属性，用于显示隐藏托盘图标
        public static readonly DependencyProperty AreaVisibilityProperty =
            DependencyProperty.Register("AreaVisibility", typeof(bool), typeof(GWindow));
        //注册AreaMenuItems属性，用于托盘右键在单的列表
        public static readonly DependencyProperty AreaMenuItemsProperty =
            DependencyProperty.Register("AreaMenuItems", typeof(List<MenuItem>), typeof(GWindow), new PropertyMetadata(new List<MenuItem>()));
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = AreaText;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                notifyIcon.Icon = GetImageSource(AreaIcon);
            }
            notifyIcon.Visible = AreaVisibility;
            if (this.AreaMenuItems != null && this.AreaMenuItems.Count > 0)
            {
                notifyIcon.ContextMenu = new ContextMenu(this.AreaMenuItems.ToArray());
            }
        }
        public List<MenuItem> AreaMenuItems
        {
            get { return (List<MenuItem>)GetValue(AreaMenuItemsProperty); }
            set { SetValue(AreaMenuItemsProperty, value); }
        }
        public ImageSource AreaIcon
        {
            get { return (ImageSource)GetValue(AreaIconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public string AreaText
        {
            get { return (string)GetValue(AreaTextProperty); }
            set { SetValue(AreaTextProperty, value); }
        }
        public bool AreaVisibility
        {
            get { return (bool)GetValue(AreaVisibilityProperty); }
            set { SetValue(AreaVisibilityProperty, value); }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            AreaMenuItems.Clear();
        }
        private static Drawing.Icon GetImageSource(ImageSource icon)
        {
            if (icon == null)
            {
                return null;
            }
            Uri iconUri = new Uri(icon.ToString());
            return new Drawing.Icon(System.Windows.Application.GetResourceStream(iconUri).Stream);
        }
    }
}
