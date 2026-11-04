using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using Bit_Master_TestApp.ViewModels.ModalViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Bit_Master_TestApp.Models.Messages;

namespace Bit_Master_TestApp.ViewModels;

public partial class CarsViewModel(
    IDbContextFactory<AppDbContext> dbFactory,
    IDialogService dialogService,
    IMessenger messenger) : BaseViewModel<Car>(dbFactory, dialogService)
{
    [ObservableProperty] private Car? _selectedCar;
    private readonly IMessenger _messenger = messenger;

    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedCar == null) return;
        if (!await RemoveByIdAsync(SelectedCar.Id)) {
            _dialogService.ShowWarning("Нельзя удалить эту машину, так как она используется в экипажах", "Ошибка");
        }
    }

    [RelayCommand]
    private async Task NavigateToCreate()
    {
        var vm = new CarModalViewModel(_dbFactory, dialogService);
        if (_dialogService.ShowEditor(vm))
            await LoadAsync();
    }

    [RelayCommand]
    private async Task NavigateToUpdate(Car? car)
    {
        if (car == null) return;

        var vm = new CarModalViewModel(_dbFactory, dialogService, car);
        if (_dialogService.ShowEditor(vm))
        {
            var message = new CrewsUpdatedMessages
            {
                UpdatedCarIds = [car.Id]
            };
            _messenger.Send(message);
            await LoadAsync();
        }
    }

}
