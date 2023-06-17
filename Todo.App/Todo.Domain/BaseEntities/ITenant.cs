namespace Todo.Domain.BaseEntities;

public interface ITenant
{
    Guid? TenantId { get; set; }
}