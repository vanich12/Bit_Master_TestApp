using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels;

public partial class CrewModalViewModel : BaseModalViewModel<Crew>
{
    public ObservableCollection<Car> AvailableCars { get; } = [];
    public ObservableCollection<Driver> AvailableDrivers { get; } = [];
    private readonly IDialogService _dialogService;

    [ObservableProperty] private Car? selectedCar;

    [ObservableProperty] private Driver? selectedDriver;
    public CrewModalViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService, Crew? crew = null)
        : base(dbFactory, dialogService, crew)
    {
        _dialogService = dialogService;
        _ = LoadListsAsync();
    }

    partial void OnSelectedCarChanged(Car? value)
    {
        if (value != null) 
            Item.CarId = value.Id;
    }

    partial void OnSelectedDriverChanged(Driver? value)
    {
        if (value != null) 
            Item.DriverId = value.Id;
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

        if (!IsNew)
        {
            SelectedCar = AvailableCars.FirstOrDefault(c => c.Id == Item.CarId);
            SelectedDriver = AvailableDrivers.FirstOrDefault(d => d.Id == Item.DriverId);
        }
    }

    protected override async Task<bool> ValidateAsync()
    {
        if (SelectedCar == null || SelectedDriver == null)
        {
            _dialogService.ShowWarning("Выберите машину и водителя.", "Не выбраны параметры");
            return false;
        }

        await using var db = await _dbFactory.CreateDbContextAsync();

        bool exists = await db.Crews.AnyAsync(c =>
            c.CarId == SelectedCar.Id &&
            c.DriverId == SelectedDriver.Id &&
            c.Id != Item.Id);

        if (exists)
        {
            _dialogService.ShowWarning("Экипаж с такой машиной и таким водителем уже существует.","Дубликат");
            return false;
        }

        return true;
    }
}

