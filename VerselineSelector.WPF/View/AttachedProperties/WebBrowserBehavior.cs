using System;
using System.Net.Http;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;

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

    private static async void OnBindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WebView2 browser)
        {
            await browser.EnsureCoreWebView2Async();            

            var uri = (string)e.NewValue;
            var html = "<!DOCTYPE html><html lang=\"nl-be\"><head /><style>p {font-family: \"Segoe UI\" ;}</style><body><p>Even geduld, de bijlage wordt geladen...</p></body></html>";
            browser.NavigateToString(html);

            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetByteArrayAsync(uri);
                var pdfAsBase64 = Convert.ToBase64String(response);

                html = "<!DOCTYPE html><html lang=\"nl-be\"><head /><style>body {margin: 0;} object {display: block; background: #000; border: none; height: 100vh; width: 100vw;}</style><body><div>"
                    + $"<object type=\"application/pdf\" data=\"data:application/pdf;base64,{pdfAsBase64}\">"
                    + "</object></div></body></html>";                
            }
            
            browser.NavigateToString(html);
        }
    }
}
