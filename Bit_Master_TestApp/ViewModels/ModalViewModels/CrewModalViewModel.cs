using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels
{
    public partial class CrewModalViewModel : BaseModalViewModel<Crew>
    {
        public ObservableCollection<Car> AvailableCars { get; } = [];
        public ObservableCollection<Driver> AvailableDrivers { get; } = [];

        [ObservableProperty] private Car? selectedCar;

        [ObservableProperty] private Driver? selectedDriver;

        // Частичные методы генератора (вызываются при изменении SelectedCar/SelectedDriver)
        partial void OnSelectedCarChanged(Car? value)
        {
            // Обновляем ID в модели, чтобы при SetValues(Crew) сохранилась связь
            if (value != null) 
                Item.CarId = value.Id;
        }

        partial void OnSelectedDriverChanged(Driver? value)
        {
            if (value != null) 
                Item.DriverId = value.Id;
        }

        public CrewModalViewModel(IDbContextFactory<AppDbContext> dbFactory, Crew? crew = null)
            : base(dbFactory, crew)
        {
            _ = LoadListsAsync();
        }

        private async Task LoadListsAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var cars = await db.Cars.AsNoTracking().ToListAsync();
            AvailableCars.Clear();
            foreach (var car in cars)
                AvailableCars.Add(car);

            var drivers = await db.Drivers.AsNoTracking().ToListAsync();
            AvailableDrivers.Clear();
            foreach (var driver in drivers)
                AvailableDrivers.Add(driver);

            // 3. Устанавливаем выбранные элементы, если это редактирование
            if (!IsNew)
            {
                SelectedCar = AvailableCars.FirstOrDefault(c => c.Id == Item.CarId);
                SelectedDriver = AvailableDrivers.FirstOrDefault(d => d.Id == Item.DriverId);
            }
        }
    }
}
