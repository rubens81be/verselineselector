using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VerselineSelector.WPF.View.ValueConverters;

public class BoolToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter switch
        {
            "Search" => (bool)value ? Brushes.Black : Brushes.LightGray,
            _ => (bool)value ? Brushes.Green : Brushes.Red
        };        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
