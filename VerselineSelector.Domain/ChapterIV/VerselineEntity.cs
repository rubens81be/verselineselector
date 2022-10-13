using VerselineSelector.Domain.Patient;

namespace VerselineSelector.Domain.ChapterIV;

public class VerselineEntity
{
    public string ParagraphName { get; set; } = string.Empty;


    public int Sequence { get; set; }

    public int VerselineNumber { get; set; }

    public VerselineType Type { get; set; } = VerselineType.Undefined;

    public int ParentSequence { get; set; }

    public int Level { get; set; }

    public bool HasCheckbox { get; set; } = false;

    public int MinimumChecks { get; set; }

    public int MinimumAge { get; set; }

    public AgeUnitType MinimumAgeUnit { get; set; } = AgeUnitType.Undefined;

    public int MaximumAge { get; set; }

    public AgeUnitType MaximumAgeUnit { get; set; } = AgeUnitType.Undefined;

    public SexType SexRestriction { get; set; } = SexType.Undefined;

    public bool AnnexMandatory { get; set; } = false;

    public string Text { get; set; } = string.Empty;
    
    public virtual ParagraphEntity Paragraph { get; set; } = new();

    public virtual VerselineEntity? Parent { get; }

    public virtual ICollection<VerselineEntity>? Children { get; }

    public virtual ICollection<DocumentEntity>? Documents { get; }
}