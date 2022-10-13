using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VerselineSelector.DAL.Core;
using VerselineSelector.Domain.Patient;
using VerselineSelector.WPF.Model;
using VerselineSelector.WPF.Services;
using VerselineSelector.WPF.ViewModel.Enums;

namespace VerselineSelector.WPF.ViewModel;

[ObservableObject]
public partial class MainWindowViewModel
{    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHighlightingService _highlightingService;
    
    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsComplete))]
    private PatientEntity? _currentPatient;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSearch))]
    [NotifyPropertyChangedFor(nameof(IsComplete))]
    private ObservableParagraph? _currentParagraph;
    
    [ObservableProperty]
    private ObservableCollection<ObservableParagraph> _paragraphs = new();

    [ObservableProperty]
    private ObservableCollection<ObservableVerseline> _verselines = new();

    [ObservableProperty]
    private ObservableCollection<ObservableVerseline> _selectedVerselines = new();

    [ObservableProperty]
    private ObservableCollection<PatientEntity> _patients = new();

    [ObservableProperty]
    private int _tabItemIndex;

    public bool CanSearch => CurrentParagraph is not null;

    public bool AreAllVerselineConditionsSatisfied
    {
        get
        {
            foreach (var verseline in _verselines)
            {
                if (!verseline.IsAgeAndSexConditionSatisfied)
                {
                    return false;                    
                }
            }
            return true;
        }
    }

    public bool IsTopMostMinimumCheckConditionSatisfied
    {
        get
        {
            foreach (var verseline in _verselines)
            {
                if (verseline.Verseline.MinimumChecks > 0)
                {
                    return verseline.IsMinimumCheckConditionSatisfied;                    
                }
            }
            return true;
        }
    }

    public bool IsComplete => CurrentParagraph is not null 
        && AreAllVerselineConditionsSatisfied 
        && IsTopMostMinimumCheckConditionSatisfied;
    
    public MainWindowViewModel(IUnitOfWork unitOfWork, IHighlightingService highlightingService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _highlightingService = highlightingService ?? throw new ArgumentNullException(nameof(highlightingService));
    }
    
    partial void OnCurrentParagraphChanged(ObservableParagraph? value)
    {
        LoadVerselineDataCommand.Execute(value!.Paragraph.Name);
        if (value!.RequestFormA is null)
        {
            TabItemIndex = 0;
        }
    }
    
    partial void OnCurrentPatientChanged(PatientEntity? value)
    {
        foreach (var verseline in _verselines)
        {
            verseline.PatientContext = value;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        HighlightParagraphText(value);
    }
    
    private void HighlightParagraphText(string textToFind)
    {
        foreach(ObservableVerseline verseline in Verselines)
        {
            verseline.InlineCollection = _highlightingService.Highlight(verseline.Verseline.Text, textToFind);
        }
    }

    [RelayCommand]
    private async Task LoadInitialDataAsync()
    {
        if (LoadInitialDataCommand.ExecutionTask is not null
            && !LoadInitialDataCommand.ExecutionTask.IsCompleted)
        {
            return;
        }

        var patients = await _unitOfWork.Patients.GetAll();
        Patients = new ObservableCollection<PatientEntity>(patients.OrderBy(p => p.Age).ThenBy(p => p.Sex).ThenBy(p => p.Name).ToList());

        var paragraphs = await _unitOfWork.Paragraphs.GetAllSortedAsc();
        Paragraphs = new ObservableCollection<ObservableParagraph>(paragraphs.Select(p => new ObservableParagraph(p)).OrderBy(p => p.ParagraphNumber).ToList());
    }

    [RelayCommand]
    private async Task LoadVerselineDataAsync(string? paragraph)
    {
        if (paragraph is null)
        {
            throw new ArgumentNullException(nameof(paragraph));
        }

        if (LoadVerselineDataCommand.ExecutionTask is not null
            && !LoadVerselineDataCommand.ExecutionTask.IsCompleted)
        {
            return;
        }

        var verselines = await _unitOfWork.Verselines.GetAllForParagraph(paragraph);
        Verselines = new(verselines.Select(v => new ObservableVerseline(v, CurrentPatient)).ToList());

        SelectedVerselines = new();
        OnPropertyChanged(nameof(IsComplete));
    }

    [RelayCommand]
    private void SelectVerseline(ObservableVerseline verseline)
    {
        _ = verseline ?? throw new ArgumentNullException(nameof(verseline));
        
        var parent = _verselines.First(c => c.Verseline.Sequence == verseline.Verseline.ParentSequence);
        if(verseline.IsChecked)
        {
            SelectedVerselines.Add(verseline);
            UpgradeParents(verseline);
        }
        else
        {
            SelectedVerselines.Remove(verseline);
            DowngradeParents(verseline);
        }

        OnPropertyChanged(nameof(IsComplete));
        OnPropertyChanged(nameof(SelectedVerselines));
    }

    private void UpgradeParents(ObservableVerseline verseline)
    {
        UpdateParent(verseline, PropagationType.PROPAGATE_UP);
    }

    private void DowngradeParents(ObservableVerseline verseline)
    {
        UpdateParent(verseline, PropagationType.PROPAGATE_DOWN);
    }

    private void UpdateParent(ObservableVerseline verseline, PropagationType propagationType)
    {
        var parent = Verselines.Where(c => c.Verseline.Sequence == verseline.Verseline.ParentSequence).FirstOrDefault();
        if (parent is null)
        {
            return;
        }

        parent.ChildCheckedCounter += (int)propagationType;

        var needsPropagation = (parent.Verseline.MinimumChecks == 0) 
            || ((propagationType == PropagationType.PROPAGATE_DOWN) 
                ? parent.Verseline.MinimumChecks == parent.ChildCheckedCounter + 1
                : parent.Verseline.MinimumChecks == parent.ChildCheckedCounter);

        if(needsPropagation)
        {
            UpdateParent(parent, propagationType);
        }
    }
    
    [RelayCommand]
    private void AddAnnex(ObservableVerseline verseline)
    {
    }
}