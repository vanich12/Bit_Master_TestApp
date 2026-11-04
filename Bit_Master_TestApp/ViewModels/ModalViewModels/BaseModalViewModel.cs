using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels;

public abstract partial class BaseModalViewModel<T>(IDbContextFactory<AppDbContext> dbFactory, IDialogService dialogService, T? existing = null)
    : ObservableObject
    where T : class, IEntity, new()
{
    protected abstract Task<bool> ValidateAsync();


    protected readonly IDbContextFactory<AppDbContext> _dbFactory = dbFactory;
    public T Item { get; } = existing ?? Activator.CreateInstance<T>();
    public bool IsNew { get; set; } = existing == null;

    [RelayCommand]
    private async Task Update()
    {
        if (!await ValidateAsync()) return;
        await using var db = await _dbFactory.CreateDbContextAsync();
        var old = await db.Set<T>().FindAsync(Item.Id);

        if (old == null)
        {
            MessageBox.Show("Запись была удалена.", "Не найдено",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            CloseWindow(false);
            return;
        }

        db.Entry(old).CurrentValues.SetValues(Item);
        await db.SaveChangesAsync();
        CloseWindow(true);
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        if (!await ValidateAsync()) return;
        await using var dbContext = await _dbFactory.CreateDbContextAsync();
        DbSet<T> dbSet = dbContext.Set<T>();

        await dbSet.AddAsync(Item);
        await dbContext.SaveChangesAsync();
        CloseWindow(true);
    }

    [RelayCommand]
    private void Cancel() => CloseWindow(false);
    private void CloseWindow(bool r)
    {
        foreach (Window w in Application.Current.Windows)
            if (w.DataContext == this) { w.DialogResult = r; return; }
    }
}