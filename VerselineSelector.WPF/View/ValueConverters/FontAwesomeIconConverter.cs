using FontAwesome.Sharp;
using System;
using System.Globalization;
using System.Windows.Data;
using VerselineSelector.Domain.Patient;

namespace VerselineSelector.WPF.View.ValueConverters;

public class FontAwesomeIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SexType sexValue)
        {
            return sexValue switch
            {
                SexType.Female => IconChar.Venus,
                SexType.Male => IconChar.Mars,
                _ => IconChar.MarsAndVenus
            };
        }

        if (value is bool boolValue)
        {
            return boolValue ? IconChar.CircleCheck : IconChar.CircleXmark;
        }

        return IconChar.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
