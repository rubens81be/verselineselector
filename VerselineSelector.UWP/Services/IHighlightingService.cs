using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Media;

namespace VerselineSelector.WPF.Services;

public interface IHighlightingService
{
    List<Inline> Highlight(string textToParse, string textToHighlight, Brush? backgroundBrush = null, bool tokenize = true);
}
