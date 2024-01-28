using KackelboControl_API.DTOs.Application;
using System.ComponentModel.DataAnnotations;

namespace KackelboControl_API.Database.Entities;

public class EggCount
{
    public EggCount()
    {
        
    }
    public EggCount(EggCountDto eggCountDto)
    {
        CountDate = eggCountDto.CountDate;
        Count = eggCountDto.Count;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public DateOnly CountDate { get; set; }

    [Required]
    public int Count { get; set; }

}
