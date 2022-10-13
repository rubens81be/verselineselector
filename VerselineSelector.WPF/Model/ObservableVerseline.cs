using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using VerselineSelector.Domain.ChapterIV;
using VerselineSelector.Domain.Patient;

namespace VerselineSelector.WPF.Model;

public partial class ObservableVerseline : ObservableObject
{
    private const double INDENTATION = 15.0;
    private const double ICON_INDENTATION = 20.0;
    private readonly VerselineEntity _verseline;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAgeRestrictionSatisfied))]
    [NotifyPropertyChangedFor(nameof(IsSexRestrictionSatisfied))]
    [NotifyPropertyChangedFor(nameof(IsAgeAndSexConditionSatisfied))]
    [NotifyPropertyChangedFor(nameof(CanCheck))]
    private PatientEntity? _patientContext;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAgeAndSexConditionSatisfied))]
    private bool _isChecked;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMinimumCheckConditionSatisfied))]
    [NotifyPropertyChangedFor(nameof(IsAgeAndSexConditionSatisfied))]
    private int _childCheckedCounter = 0;
    
    [ObservableProperty]
    private IList<Inline> _inlineCollection;

    public VerselineEntity Verseline => _verseline;

    public double IndentationLevel => ((_verseline.Level - (_verseline.HasCheckbox ? 2.0 : 1.0)) * INDENTATION) + (HasAdditionalInformation ? 0.0 : ICON_INDENTATION);
    
    public bool HasAdditionalInformation => _verseline.AnnexMandatory
        || IsParagraphTitle
        || (!_verseline.SexRestriction.Equals(SexType.Undefined) && !_verseline.SexRestriction.Equals(SexType.Unknown))
        || !_verseline.Type.Equals(VerselineType.Undefined)
        || !String.IsNullOrEmpty(AgeAuthorizationAsString);
    
    public bool IsParagraphTitle => _verseline.Sequence == 1;
    
    public bool IsAgeRestricted => IsMinimumAgeRestricted || IsMaximumAgeRestricted;

    public bool IsMinimumAgeRestricted => _verseline.MinimumAgeUnit is not AgeUnitType.Undefined;
    
    public bool IsMaximumAgeRestricted => _verseline.MaximumAgeUnit is not AgeUnitType.Undefined;

    public bool IsSexRestricted => _verseline.SexRestriction is not SexType.Undefined;

    public bool IsMinimumAgeRestrictionSatisfied => !IsMinimumAgeRestricted || (_patientContext is not null && _patientContext.Age >= _verseline.MinimumAge);

    public bool IsMaximumAgeRestrictionSatisfied => !IsMaximumAgeRestricted || (_patientContext is not null && _patientContext.Age <= _verseline.MaximumAge);

    public bool IsAgeRestrictionSatisfied => IsMinimumAgeRestrictionSatisfied && IsMaximumAgeRestrictionSatisfied;

    public bool IsSexRestrictionSatisfied => !IsSexRestricted || (_patientContext is not null && _patientContext.Sex.Equals(_verseline.SexRestriction));
    
    public bool IsAgeAndSexConditionSatisfied => _verseline.HasCheckbox
        ? !IsChecked || (IsSexRestrictionSatisfied && IsAgeRestrictionSatisfied)
        : IsSexRestrictionSatisfied && IsAgeRestrictionSatisfied;
    
    public bool IsMinimumCheckConditionSatisfied => _verseline.MinimumChecks > 0 && _verseline.MinimumChecks <= _childCheckedCounter;
    
    public bool CanCheck => _verseline.HasCheckbox && IsSexRestrictionSatisfied && IsAgeRestrictionSatisfied;

    public string AgeAuthorizationAsString 
    { 
        get
        {
            var sb = new StringBuilder();
            if(_verseline.MinimumAge > 0)
            {
                sb.Append(_verseline.MinimumAge);
            }

            if(_verseline.MaximumAge > 0)
            {
                sb.Append('-');
                sb.Append(_verseline.MaximumAge);
            } 
            else if(_verseline.MinimumAge > 0) 
            {
                sb.Append('+');
            }

            return sb.ToString();
        } 
    }
    
    public string MinimalChecksAsString => _verseline.MinimumChecks == 0 ? string.Empty : $"[{_verseline.MinimumChecks}]";

    public ObservableVerseline(VerselineEntity verseline, PatientEntity? patientContext = null)
    {
        _verseline = verseline ?? throw new ArgumentNullException(nameof(verseline));
        _patientContext = patientContext;
        _inlineCollection = new List<Inline>
        {
            new Run(_verseline.Text)
        };
    }
    
    public override string ToString()
    {
        return $"{_verseline.Sequence} - C: {IsAgeAndSexConditionSatisfied}, A: {IsAgeRestrictionSatisfied}, S: {IsSexRestrictionSatisfied}";
    }
}