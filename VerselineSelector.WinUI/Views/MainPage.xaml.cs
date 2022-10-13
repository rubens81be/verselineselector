using Microsoft.UI.Xaml.Controls;

using VerselineSelector.WinUI.ViewModels;

namespace VerselineSelector.WinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
