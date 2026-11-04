using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using Bit_Master_TestApp.Models.Messages;
using Bit_Master_TestApp.Services;
using Bit_Master_TestApp.ViewModels.ModalViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.ViewModels
{
    public partial class CrewsViewModel : BaseViewModel<Crew>, IRecipient<CrewsUpdatedMessages>
    {
        [ObservableProperty] private Crew? _selectedCrew;
        private readonly IMessenger _messanger;
        private readonly IDialogService _dialogService;

        public CrewsViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService, IMessenger messanger)
            : base(dbFactory, dialogService)
        {
            _messanger = messanger;
            _messanger.Register(this);
            _messanger = messanger;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedCrew == null) return;
            _ = RemoveById(SelectedCrew.Id);
        }

        [RelayCommand]
        private async Task CreateCrewAsync()
        {
            var vm = new CrewModalViewModel(_dbFactory, _dialogService);
            if (_dialogService.ShowEditor(vm))
                await LoadAsync();
        }

        [RelayCommand]
        private async Task EditCrewAsync(Crew? crew)
        {
            if (crew == null) return;

            var vm = new CrewModalViewModel(_dbFactory, _dialogService, crew);
            if (_dialogService.ShowEditor(vm))
                await LoadAsync();
        }

        protected override async Task LoadAsync()
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var dbSet = await dbContext.Crews.AsNoTracking()
                .Include(c => c.Car)
                .Include(d => d.Driver).ToListAsync();

            Items.Clear();
            foreach (var item in dbSet)
                Items.Add(item);
        }

        private async Task<IEnumerable<Crew>> GetByIds(List<int>? carsIds, List<int>? driverIds)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var updatedCrews = await dbContext.Crews
                .Where(x => carsIds == null || carsIds.Contains(x.CarId) || driverIds == null || driverIds.Contains(x.DriverId))
                .AsNoTracking()
                .Include(x => x.Car)
                .Include(x => x.Driver).ToListAsync();
            return updatedCrews;
        }

        public async void Receive(CrewsUpdatedMessages message)
        {
            var updatedCrews = await GetByIds(message.UpdatedCarIds,message.UpdatedDriverIds);

            foreach (var freshCrew in updatedCrews)
            {
                var existing = Items.FirstOrDefault(c => c.Id == freshCrew.Id);

                if (existing != null)
                {
                    var index = Items.IndexOf(existing);
                    Items[index] = freshCrew;
                }
                else
                    Items.Add(freshCrew);
                
            }
        }
    }
}
