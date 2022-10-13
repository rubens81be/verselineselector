using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VerselineSelector.WPF.View.ValueConverters;

public class MarginConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var level = value is IConvertible lvl ? lvl.ToDouble(culture.NumberFormat) : 1.0d;
        var indentation = parameter is IConvertible indent ? indent.ToDouble(culture.NumberFormat) : 1.0d;

        return new Thickness(level * indentation, 0, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
