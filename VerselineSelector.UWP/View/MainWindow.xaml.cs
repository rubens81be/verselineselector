using FontAwesome.Sharp;
using System;
using System.Windows;
using VerselineSelector.WPF.ViewModel;

namespace VerselineSelector.WPF.View;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        Icon = IconChar.BookReader.ToImageSource();
        InitializeComponent();

        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadInitialDataCommand.Execute(null);
    }
}
