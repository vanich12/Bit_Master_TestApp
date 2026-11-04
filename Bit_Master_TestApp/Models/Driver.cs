using Bit_Master_TestApp.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Bit_Master_TestApp.Models;
public class Driver: IEntity
{
    public int Id { get; set; }

    [Required] [MaxLength(150)] 
    public string FullName { get; set; } = string.Empty;

    [Required] [MaxLength(50)] 
    public string LicenseNumber { get; set; } = string.Empty;

    [MaxLength(20)] 
    public string PhoneNumber { get; set; } = string.Empty;
}