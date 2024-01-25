using KackelboControl_API.DTOs.Application;
using System.ComponentModel.DataAnnotations;

namespace KackelboControl_API.Database.Entities;

public class SensorTriggers
{
    public SensorTriggers()
    {
        
    }
    public SensorTriggers(UpdateSensorTriggersDto updateSensorTriggers, DateTime created)
    {
        LightOnTime = updateSensorTriggers.LightOnTime;
        LightOffTime = updateSensorTriggers.LightOffTime;
        MinTemp = updateSensorTriggers.MinTemp;
        MaxTemp = updateSensorTriggers.MaxTemp;
        UseSunLight = updateSensorTriggers.UseSunLight;
        Created = created;
    }


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
    public DateTime? LastSynced { get; set; }
}

