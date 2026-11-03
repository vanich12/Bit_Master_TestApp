using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels
{
    public partial class CarModalViewModel(IDbContextFactory<AppDbContext> dbFactory, Car? car = null)
        : BaseModalViewModel<Car>(dbFactory, car)
    {
    }
}
