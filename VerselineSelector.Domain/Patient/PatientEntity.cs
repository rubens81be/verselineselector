namespace VerselineSelector.Domain.Patient;
public class PatientEntity
{
    public string PatientNumber { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public DateOnly DateOfBirth { get; private set; } = DateOnly.MinValue;

    public SexType Sex { get; private set; } = SexType.Unknown;

    public int Age => DateTime.Now.Year - DateOfBirth.Year;

    public override string ToString() => $"{Name} ({Age})";
}
