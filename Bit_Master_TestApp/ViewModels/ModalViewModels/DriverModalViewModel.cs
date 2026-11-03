using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bit_Master_TestApp.Data;
using Bit_Master_TestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.ViewModels.ModalViewModels
{
    public class DriverModalViewModel(IDbContextFactory<AppDbContext> dbFactory, Driver? item = null) : BaseModalViewModel<Driver>(dbFactory, item)
    {
    }
}
