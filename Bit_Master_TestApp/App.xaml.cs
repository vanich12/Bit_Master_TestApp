using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Services;
using Bit_Master_TestApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;


namespace Bit_Master_TestApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
         _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context,services) =>
        {
            var databaseDirectory = Path.Combine(AppContext.BaseDirectory, "Database");
            Directory.CreateDirectory(databaseDirectory);
            var databasePath = Path.Combine(databaseDirectory, "app.db");

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={databasePath}"));

            services.AddHostedService<BackgroundDeleteService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();

            services.AddTransient<CarsViewModel>();
            services.AddTransient<CrewsViewModel>();
            services.AddTransient<DriversViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();

            services.AddTransient<CarsViewModel>();
            services.AddTransient<DriversViewModel>();
            services.AddTransient<CrewsViewModel>();

        }).Build();

        await _host.StartAsync();

        var dbFactory = _host.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await DataSeeder.SeedAsync(dbFactory);

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}