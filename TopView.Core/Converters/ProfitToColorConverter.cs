using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Core.Converters
{
    public class ProfitToColorConverter : IValueConverter
    {
        public Color PositiveColor { get; set; } = Colors.Green;
        public Color NegativeColor { get; set; } = Colors.Red;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d)
            {
                if (d < 0)
                { 
                    return NegativeColor;
                }
            }
            
            return PositiveColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
