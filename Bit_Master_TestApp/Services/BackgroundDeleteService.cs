using System.Windows;
using Bit_Master_TestApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace Bit_Master_TestApp.Services;

public class BackgroundDeleteService(IDbContextFactory<AppDbContext> dbFactory, IMessenger messenger, IDialogService dialogService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await using var dbContext = await dbFactory.CreateDbContextAsync(stoppingToken);
                var threshold = DateTime.UtcNow.AddSeconds(-10);

                var currentEditingId = dialogService.CurrentEditingCrewId;

                var inactiveCrews = await dbContext.Crews
                    .Where(x => !x.Active
                                && x.UpdatedAt < threshold
                                && (currentEditingId == null || x.Id != currentEditingId))
                    .ToListAsync(stoppingToken);

                if (!inactiveCrews.Any()) continue;

                dbContext.Crews.RemoveRange(inactiveCrews);
                await dbContext.SaveChangesAsync(stoppingToken);

                var removedIds = inactiveCrews.Select(x => x.Id).ToList();
                var message = new CrewsUpdatedMessages
                {
                    RemovedCrewId = removedIds
                };

                Application.Current.Dispatcher.Invoke(() =>
                {
                    messenger.Send(message);
                });
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в фоновом сервисе: {ex.Message}");
        }
    }
}
