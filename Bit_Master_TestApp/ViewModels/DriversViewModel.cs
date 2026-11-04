using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using Bit_Master_TestApp.Models.Messages;
using Bit_Master_TestApp.ViewModels.ModalViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.ViewModels;

public partial class DriversViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService, IMessenger messanger) : BaseViewModel<Driver>(dbFactory, dialogService)
{
    [ObservableProperty] private Driver? _selectedDriver;
    private readonly IMessenger _messenger = messanger;

    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedDriver == null) return;
        if (!await RemoveByIdAsync(SelectedDriver.Id))
        {
            _dialogService.ShowWarning("Нельзя запись о данном водителе,так как он состоит в экипаже", "Ошибка");
        }
    }

    [RelayCommand]
    private async Task NavigateToCreate()
    {
        var vm = new DriverModalViewModel(_dbFactory, dialogService);
        if (_dialogService.ShowEditor(vm))
            await LoadAsync();
    }

    [RelayCommand]
    private async Task NavigateToUpdate(Driver? driver)
    {
        if (driver == null) return;

        var vm = new DriverModalViewModel(_dbFactory, dialogService, driver);
        if (_dialogService.ShowEditor(vm))
        {
            var message = new CrewsUpdatedMessages
            {
                UpdatedDriverIds = [driver.Id]
            };
            _messenger.Send(message);
            await LoadAsync();
        }
    }
}