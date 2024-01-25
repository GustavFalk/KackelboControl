using System.ComponentModel.DataAnnotations;

namespace KackelboControl_API.Database.Entities;

public class SensorTriggers
{
    [Key]
    public int Id { get; set; }

    [Required]
    public TimeOnly LightOnTime { get; set; }
    [Required]
    public TimeOnly LightOffTime { get; set; }
    [Required]
    public decimal MinTemp { get; set; }
    [Required]
    public decimal MaxTemp { get; set; }
    [Required]
    public bool UseSunLight { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public DateTime LastSynced { get; set; }
}

