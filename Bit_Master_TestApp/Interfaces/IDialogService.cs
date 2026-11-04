using CommunityToolkit.Mvvm.ComponentModel;


namespace Bit_Master_TestApp.Interfaces;

public interface IDialogService
{
    bool ShowEditor<T>(T viewModel) where T : ObservableObject;
    public void ShowWarning(string message, string title);
    public int? CurrentEditingCrewId { get; }
}

