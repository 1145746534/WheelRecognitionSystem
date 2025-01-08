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
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //bool.TryParse(value.ToString(), out bool result);
            if (value.ToString() == "0")
            {
                return ImageProcessingHelper.GetLinearGradientBrush(Colors.Red);
            }
            else if (value.ToString() == "1")
            {
                return ImageProcessingHelper.GetLinearGradientBrush(Colors.LightGreen);
            }
            else
            {
                return ImageProcessingHelper.GetLinearGradientBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
