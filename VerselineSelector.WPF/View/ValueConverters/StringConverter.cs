using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using VerselineSelector.WPF.Model;

namespace VerselineSelector.WPF.View.ValueConverters;

public class StringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ObservableCollection<ObservableVerseline> collection)
        {
            if (collection.Count == 0)
            {
                return "Geen versregels geselecteerd.";
            }

            return (collection.Count == 1 ? "Versregel: " : "Versregels: ") + string.Join(";", collection.OrderBy(v => v.Verseline.VerselineNumber).Select(v => v.Verseline.VerselineNumber.ToString()).ToList());
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
