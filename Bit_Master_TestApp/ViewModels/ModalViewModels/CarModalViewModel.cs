using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels
{
    public partial class CarModalViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService, Car? car = null)
        : BaseModalViewModel<Car>(dbFactory, dialogService, car)
    {
        protected override Task<bool> ValidateAsync()
        {
            if (string.IsNullOrWhiteSpace(Item.Model))
            {
                dialogService.ShowWarning("Введите модель автомобиля.", "Ошибка валидации");
                return Task.FromResult(false);
            }

            if (string.IsNullOrWhiteSpace(Item.LicensePlate))
            {
                dialogService.ShowWarning("Введите госномер автомобиля.", "Ошибка валидации");
                return Task.FromResult(false);
            }

            if (Item.Year is null)
            {
                dialogService.ShowWarning("Введите корректную дату выпуска автомобиля.", "Ошибка валидации");
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
