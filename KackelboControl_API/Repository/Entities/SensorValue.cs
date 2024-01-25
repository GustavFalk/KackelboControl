using System.ComponentModel.DataAnnotations;

namespace KackelboControl_API.Repository.Entities;

public class SensorValue
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public TimeOnly ArduinoTime { get; set; }
    [Required]
    public decimal TempIndoors { get; set; }
    [Required]
    public decimal TempOutdoor { get; set; }
}

