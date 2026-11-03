using CommunityToolkit.Mvvm.ComponentModel;


namespace Bit_Master_TestApp.Interfaces
{
    public interface IDialogService
    {
        bool ShowEditor<T>(T viewModel) where T : ObservableObject;
        int? CurrentEditingCrewId { get; }
    }
}
