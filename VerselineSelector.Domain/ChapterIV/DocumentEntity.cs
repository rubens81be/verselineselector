namespace VerselineSelector.Domain.ChapterIV;

public class DocumentEntity
{
    public string ParagraphName { get; set; } = string.Empty;

    public int VerselineSequence { get; set; }

    public int DocumentSequence { get; set; }

    public string Uri { get; set; } = string.Empty;

    public virtual ParagraphEntity Paragraph { get; set; } = new();
    
    public virtual VerselineEntity Verseline { get; set; } = new();
}