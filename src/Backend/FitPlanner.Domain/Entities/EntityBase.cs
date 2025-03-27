namespace FitPlanner.Domain.Entities;

public class EntityBase
{
    public long Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedOn { get; set; }
}