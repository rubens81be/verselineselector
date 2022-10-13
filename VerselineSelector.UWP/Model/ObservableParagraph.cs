using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using VerselineSelector.Domain.ChapterIV;

namespace VerselineSelector.WPF.Model;

public partial class ObservableParagraph : ObservableObject
{
    [ObservableProperty]
    private ParagraphEntity _paragraph;
    public ObservableParagraph(ParagraphEntity paragraph)
    {
        _paragraph = paragraph;
    }

    public int ParagraphNumber => int.Parse(_paragraph.Name);

    public DocumentEntity? RequestFormA => _paragraph.Documents?.FirstOrDefault();
}
