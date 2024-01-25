using System.ComponentModel.DataAnnotations;

namespace KackelboControl_API.Repository.Entities;

public class Sunlight
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateOnly Date { get; set; }
    [Required]
    public TimeOnly Sunrise { get; set; }
    [Required]
    public TimeOnly Sunset { get; set; }
}
