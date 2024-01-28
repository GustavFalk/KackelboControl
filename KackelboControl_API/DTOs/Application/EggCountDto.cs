using KackelboControl_API.Database.Entities;

namespace KackelboControl_API.DTOs.Application;

public class EggCountDto
{
    public EggCountDto()
    {

    }

    public EggCountDto(EggCount eggCount)
    {
        Count = eggCount.Count;
        CountDate = eggCount.CountDate;
    }

    public DateOnly CountDate { get; set; }
    public int Count { get; set; }
}
