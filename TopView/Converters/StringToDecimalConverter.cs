using System.Globalization;

namespace TopView.Converters
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string text && decimal.TryParse(text, out var result))
                return result;
            return 0m;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value?.ToString();
    }
}
