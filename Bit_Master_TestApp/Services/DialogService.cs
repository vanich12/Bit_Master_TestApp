using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Bit_Master_TestApp.Services;

public class DialogService: IDialogService
{
    public bool ShowEditor<T>(T viewModel) where T : ObservableObject
    {
        var window = new EditorWindow
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };
        return window.ShowDialog() == true;
    }

    public void ShowWarning(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}

