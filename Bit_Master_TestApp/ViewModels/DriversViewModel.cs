using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using Bit_Master_TestApp.ViewModels.ModalViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Bit_Master_TestApp.ViewModels;

public partial class DriversViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService) : BaseViewModel<Driver>(dbFactory, dialogService)
{
    [ObservableProperty] private Driver? _selectedDriver;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedDriver == null) return;
        _ = RemoveById(SelectedDriver.Id);
    }

    [RelayCommand]
    private async Task EditDriverAsync(Driver? driver)
    {
        if (driver == null) return;

        var vm = new DriverModalViewModel(_dbFactory, driver);
        if (dialogService.ShowEditor(vm))
            await LoadAsync();
    }
}

