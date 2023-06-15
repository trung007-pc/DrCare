namespace Todo.Domain.BaseEntities;

public class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
}