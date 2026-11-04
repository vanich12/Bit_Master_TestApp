using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels
{
    public class DriverModalViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService, Driver? item = null)
        : BaseModalViewModel<Driver>(dbFactory, dialogService, item)
    {
        protected override Task<bool> ValidateAsync()
        {
            if (string.IsNullOrWhiteSpace(Item.FullName))
            {
                dialogService.ShowWarning("Введите ФИО водителя.", "Ошибка валидации");
                return Task.FromResult(false);
            }

            if (string.IsNullOrWhiteSpace(Item.LicenseNumber))
            {
                dialogService.ShowWarning("Введите номер водительского удостоверения.", "Ошибка валидации");
                return Task.FromResult(false);
            }

            if (string.IsNullOrWhiteSpace(Item.PhoneNumber))
            {
                dialogService.ShowWarning("Введите корректный номер водителя.", "Ошибка валидации");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
