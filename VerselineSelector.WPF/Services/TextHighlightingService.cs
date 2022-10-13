using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace VerselineSelector.WPF.Services;

public class TextHighlightingService : IHighlightingService
{
    public List<Inline> Highlight(string toParse, string toHighlight, Brush? backgroundColor, bool tokenize)
    {
        backgroundColor ??= Brushes.Yellow;
        List<Inline> result = new()
        {
            new Run(toParse)
        };

        if (string.IsNullOrEmpty(toParse) || string.IsNullOrEmpty(toHighlight) || toParse.Length < toHighlight.Length)
        {
            return result;
        }
        
        var tokens = tokenize ? toHighlight.Split(' ', StringSplitOptions.RemoveEmptyEntries) : new string[] { toHighlight };
        foreach (var token in tokens)
        {
            foreach (var run in result.ToList().Cast<Run>())
            {
                var textToParse = run.Text;
                var foundIndex = textToParse.IndexOf(token, StringComparison.OrdinalIgnoreCase);

                if (foundIndex == -1 || run.Background is not null)
                {
                    continue;
                }

                var startIndex = 0;
                var index = result.IndexOf(run);
                result.Remove(run);

                do
                {
                    result.Insert(index++, new Run(textToParse[startIndex..foundIndex]));
                    result.Insert(index++, new Run(textToParse[foundIndex..(foundIndex + token.Length)])
                    {
                        Background = backgroundColor
                    });


                    startIndex = foundIndex + token.Length;
                    foundIndex = textToParse.IndexOf(token, startIndex, StringComparison.OrdinalIgnoreCase);
                } while (foundIndex != -1);

                result.Insert(index, new Run(textToParse[startIndex..]));
            }
        }
        
        return result;
    }
}
