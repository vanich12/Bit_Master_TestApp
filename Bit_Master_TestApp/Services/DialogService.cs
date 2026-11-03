using Bit_Master_TestApp.Interfaces;
using Bit_Master_TestApp.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bit_Master_TestApp.Services;

public class DialogService: IDialogService
{
    public int? CurrentEditingCrewId { get; }

    public bool ShowEditor<T>(T viewModel) where T : ObservableObject
    {
        var window = new EditorWindow
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };
        return window.ShowDialog() == true;
    }
}

