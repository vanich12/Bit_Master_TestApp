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
        
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var dbContext = await  dbFactory.CreateDbContextAsync(stoppingToken);
            var threshold = DateTime.UtcNow.AddSeconds(-30);

            var inactiveCrews = await dbContext.Crews.Where(x => !x.Active && x.UpdatedAt < threshold 
                                                                           && dialogService.CurrentEditingCrewId != x.Id).ToListAsync(cancellationToken: stoppingToken);

            if (!inactiveCrews.Any()) continue;

            dbContext.Crews.RemoveRange(inactiveCrews);
            await dbContext.SaveChangesAsync(stoppingToken);

            var message = new CrewsUpdatedMessages
            {
                RemovedCrewId = inactiveCrews.Select(x => x.Id).ToList()
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                messenger.Send(message);
            });
        }
    }
}
