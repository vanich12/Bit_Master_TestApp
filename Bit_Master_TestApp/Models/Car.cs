using Bit_Master_TestApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bit_Master_TestApp.Models;

public class Car: IEntity
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    public string LicensePlate { get; set; } = string.Empty;
    public int? Year { get; set; }
}

