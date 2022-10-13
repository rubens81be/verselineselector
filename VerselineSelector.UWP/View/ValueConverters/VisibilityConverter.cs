using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VerselineSelector.Domain.ChapterIV;
using VerselineSelector.Domain.Patient;

namespace VerselineSelector.WPF.View.ValueConverters;

public class VisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        if (value is string stringValue)
        {
            return string.IsNullOrEmpty(stringValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        if (value is SexType sexRestrictionTypeValue)
        {
            return sexRestrictionTypeValue switch
            {
                SexType.Undefined => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        if (value is VerselineType verselineTypeValue)
        {
            return verselineTypeValue switch
            {
                VerselineType.Undefined => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
