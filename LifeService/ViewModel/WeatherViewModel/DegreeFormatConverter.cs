using System;
using System.Globalization;
using System.Windows.Data;

namespace LifeService.ViewModel.WeatherViewModel
{
    internal enum Degree
    {
        c,
        f
    }

    /// <summary>
    /// 温度格式转换
    /// </summary>
    internal class DegreeFormatConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Degree s = (Degree)value;
            return s == (Degree)int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (!isChecked)
            {
                return null;
            }
            return (Degree)int.Parse(parameter.ToString());
        }
    }
}
