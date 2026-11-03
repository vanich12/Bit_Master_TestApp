using Bit_Master_TestApp.Interfaces;

namespace Bit_Master_TestApp.Models;
public class Crew: IEntity
{
    public int Id { get; set; }

    public int CarId { get; set; }
    public Car? Car { get; set; }

    public int DriverId { get; set; }
    public Driver? Driver { get; set; }

    public bool Active { get; set; } = true;
}

