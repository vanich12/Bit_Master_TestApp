using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.ViewModels.ModalViewModels;
using Bit_Master_TestApp.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Bit_Master_TestApp.Services;

public class DialogService: IDialogService
{
    public int? CurrentEditingCrewId { get; private set; }

    public bool ShowEditor<T>(T viewModel) where T : ObservableObject
    {
        if (viewModel is CrewModalViewModel crewVm)
            CurrentEditingCrewId = crewVm.Item.Id;

        var window = new EditorWindow
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };
        try
        {
            return window.ShowDialog() == true;
        }
        finally
        {
            CurrentEditingCrewId = null;
        }
    }

    public void ShowWarning(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}

