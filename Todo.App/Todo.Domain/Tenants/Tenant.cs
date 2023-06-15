using Todo.Domain.BaseEntities;

namespace Todo.Domain.Tenants;

public class Tenant : Entity
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDayDate { get; set; }
}