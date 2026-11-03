using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bit_Master_TestApp.ViewModels;

public partial class MainViewModel(
    CarsViewModel carsViewModel,
    DriversViewModel driversViewModel,
    CrewsViewModel crewsViewModel)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel = carsViewModel;

    [RelayCommand]
    private void NavigateToCars() => CurrentViewModel = carsViewModel;

    [RelayCommand]
    private void NavigateToDrivers() => CurrentViewModel = driversViewModel;

    [RelayCommand]
    private void NavigateToCrews() => CurrentViewModel = crewsViewModel;
}

