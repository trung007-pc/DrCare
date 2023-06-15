namespace Todo.Domain.BaseEntities;

public class SuperEntity : Entity
{
    public DateTime LastModificationTime { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime DeletionTime { get; set; } = DateTime.Now;
    public Guid CreatorId { get; set; }
}