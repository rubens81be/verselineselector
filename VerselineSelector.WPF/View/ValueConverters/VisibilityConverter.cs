using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Castle.Core.Internal;
using VerselineSelector.Domain.ChapterIV;
using VerselineSelector.Domain.Patient;
using VerselineSelector.WPF.Model;

namespace VerselineSelector.WPF.View.ValueConverters;

public class VisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        return value switch
        {
            bool                    => (bool)value ? Visibility.Visible : Visibility.Collapsed,
            string                  => string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible,
            DocumentEntity          => string.IsNullOrEmpty(((DocumentEntity)value).Uri) ? Visibility.Collapsed : Visibility.Visible,
            ObservableParagraph     => value is null ? Visibility.Collapsed : Visibility.Visible,
            SexType.Male            => Visibility.Visible,
            SexType.Female          => Visibility.Visible,
            VerselineType.Exclusion => Visibility.Visible,            
            _                       => Visibility.Collapsed
        };

        //if (value is bool boolValue)
        //{
        //    return boolValue ? Visibility.Visible : Visibility.Collapsed;
        //}

        //if (value is string stringValue)
        //{
        //    return string.IsNullOrEmpty(stringValue) ? Visibility.Collapsed : Visibility.Visible;
        //}

        //if (value is DocumentEntity documentValue)
        //{
        //    return string.IsNullOrEmpty(documentValue.Uri) ? Visibility.Collapsed : Visibility.Visible;
        //}

        //if (value is SexType sexRestrictionTypeValue)
        //{
        //    return sexRestrictionTypeValue switch
        //    {
        //        SexType.Undefined => Visibility.Collapsed,
        //        _ => Visibility.Visible
        //    };
        //}

        //if (value is VerselineType verselineTypeValue)
        //{
        //    return verselineTypeValue switch
        //    {
        //        VerselineType.Undefined => Visibility.Collapsed,
        //        _ => Visibility.Visible
        //    };
        //}

        //return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
