using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ChatGPT.Converter;

public class HeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double height && parameter is string subtract)
        {
            return height - double.Parse(subtract);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}