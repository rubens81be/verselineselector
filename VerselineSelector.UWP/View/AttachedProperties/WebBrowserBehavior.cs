using System;
using System.Windows;
using System.Windows.Controls;

namespace VerselineSelector.WPF.View.AttachedProperties;

public class WebBrowserBehavior : DependencyObject
{
    public static readonly DependencyProperty BindableSourceProperty =
    DependencyProperty.RegisterAttached("BindableSource",
                                        typeof(string),
                                        typeof(WebBrowserBehavior),
                                        new UIPropertyMetadata(null, OnBindableSourceChanged));

    public static string GetBindableSource(DependencyObject obj)
    {
        return (string)obj.GetValue(BindableSourceProperty);
    }

    public static void SetBindableSource(DependencyObject obj, string value)
    {
        obj.SetValue(BindableSourceProperty, value);
    }

    private static void OnBindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WebBrowser browser)
        {
            string? uri = e.NewValue as string;
            browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
        }
    }
}
