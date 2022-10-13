using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace VerselineSelector.WPF.View.AttachedProperties;

public class TextBlockBehavior : DependencyObject
{
    public static readonly DependencyProperty InlineSourceProperty =
    DependencyProperty.RegisterAttached("InlineSource",
                                        typeof(IList<Inline>),
                                        typeof(TextBlockBehavior),
                                        new UIPropertyMetadata(null, OnInlineSourceChanged));

    public static IList<Inline> GetInlineSource(DependencyObject obj)
    {
        return (IList<Inline>)obj.GetValue(InlineSourceProperty);
    }
    public static void SetInlineSource(DependencyObject obj, IList<Inline> value)
    {
        obj.SetValue(InlineSourceProperty, value);
    }

    private static void OnInlineSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        TextBlock textBlock = (TextBlock)d;
        IList<Inline> inlines = (IList<Inline>)e.NewValue;

        if (inlines == null)
        {
            return;
        }

        textBlock.Inlines.Clear();
        textBlock.Inlines.AddRange(inlines);
    }
}
