using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Bit_Master_TestApp.ViewModels;

public abstract partial class BaseViewModel<T> : ObservableObject where T : class , IEntity
{
    protected readonly IDbContextFactory<AppDbContext> _dbFactory;
    protected readonly IDialogService _dialogService;

    public ObservableCollection<T> Items { get; } = [];

    protected BaseViewModel(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService)
    {
        _dbFactory = dbFactory;
        _dialogService = dialogService;
        _ = LoadAsync();
    }

    protected virtual async Task LoadAsync()
    {
        await using var dbContext = await _dbFactory.CreateDbContextAsync();
        var dbSet = await dbContext.Set<T>().AsNoTracking().ToListAsync();

        Items.Clear();
        foreach (var item in dbSet)
            Items.Add(item);
    }

    protected virtual async Task<T?> FindByIdAsync(int id)
    {
        await using var dbContext = await _dbFactory.CreateDbContextAsync();
        DbSet<T> dbSet = dbContext.Set<T>();

        return await dbSet.FindAsync(id);
    }

    protected virtual async Task<bool> RemoveByIdAsync(int id)
    {
        await using var dbContext = await _dbFactory.CreateDbContextAsync();
        DbSet<T> dbSet = dbContext.Set<T>();

        var removedEntity = await dbSet.FindAsync(id);
        if (removedEntity == null) return false;

        dbSet.Remove(removedEntity);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return false;
        }

        var itemToRemove = Items.FirstOrDefault(x => x.Id == id);
        if (itemToRemove != null) Items.Remove(itemToRemove);

        return true;
    }
}

