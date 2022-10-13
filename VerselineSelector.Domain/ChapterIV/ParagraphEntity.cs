namespace VerselineSelector.Domain.ChapterIV;

public class ParagraphEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Title => string.Concat("§ ", Name);

    public string Description { get; private set; } = string.Empty;

    public AuthorisationType? AuthorisationType { get; private set; }

    public ParagaphProcessType? ProcessType { get; private set; }

    public int Version { get; private set; }

    public virtual ICollection<VerselineEntity>? Verselines { get; }

    public virtual ICollection<DocumentEntity>? Documents { get; }
}
