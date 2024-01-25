using System.ComponentModel.DataAnnotations;

namespace KackelboControl_API.Database.Entities;

public class RelayChange
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public Relay Relay { get; set; }

    [Required]
    public TimeOnly ArduinoTime { get; set; }
    [Required]
    public bool RelayOn { get; set; }

    public decimal TempIndoors { get; set; }

}

public enum Relay
{
    Lamp,
    Heater
}