using Bit_Master_TestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IDbContextFactory<AppDbContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();

        await db.Database.EnsureCreatedAsync();

        if (await db.Cars.AnyAsync())
            return;

        var cars = new List<Car>
        {
            new() { Model = "Газель Next", LicensePlate = "А123ВС 77", Year = 2020 },
            new() { Model = "Ford Transit", LicensePlate = "В456ЕК 99", Year = 2018 },
            new() { Model = "KAMAZ 5490", LicensePlate = "С789МН 50", Year = 2021 }
        };
        db.Cars.AddRange(cars);

        var drivers = new List<Driver>
        {
            new() { FullName = "Иванов Иван Иванович", LicenseNumber = "77АА123456", PhoneNumber = "+7(999)111-22-33" },
            new() { FullName = "Петров Петр Петрович", LicenseNumber = "99ББ654321", PhoneNumber = "+7(999)444-55-66" },
            new() { FullName = "Сидоров Сидор Сидорович", LicenseNumber = "50ВВ987654", PhoneNumber = "+7(999)777-88-99" }
        };
        db.Drivers.AddRange(drivers);

        await db.SaveChangesAsync();

        var crews = new List<Crew>
        {
            new() { CarId = cars[0].Id, DriverId = drivers[0].Id, Active = true },
            new() { CarId = cars[1].Id, DriverId = drivers[1].Id, Active = true },
            new() { CarId = cars[2].Id, DriverId = drivers[2].Id, Active = false }
        };
        db.Crews.AddRange(crews);

        await db.SaveChangesAsync();
    }
}
