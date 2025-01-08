using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WheelRecognitionSystem.Public.Converter
{
    public class BtuBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value!=null && value.ToString() == "1")
            {
                string hexColor = "#FF63DF63";
                Color mediaColor = (Color)ColorConverter.ConvertFromString(hexColor);
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                linearGradientBrush.StartPoint = new System.Windows.Point(0, 0);
                linearGradientBrush.EndPoint = new System.Windows.Point(0, 1);
                linearGradientBrush.GradientStops.Add(new GradientStop(mediaColor, 0.0));
                linearGradientBrush.GradientStops.Add(new GradientStop(mediaColor, 1.2));
                return linearGradientBrush;
            }
            else
            {
                string hexColor = "#FFBFBCBC";
                Color mediaColor = (Color)ColorConverter.ConvertFromString(hexColor);
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                linearGradientBrush.StartPoint = new System.Windows.Point(0, 0);
                linearGradientBrush.EndPoint = new System.Windows.Point(0, 1);
                linearGradientBrush.GradientStops.Add(new GradientStop(mediaColor, 0.0));
                linearGradientBrush.GradientStops.Add(new GradientStop(mediaColor, 1.2));               
                return linearGradientBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
