using Todo.Domain.BaseEntities;
using Todo.Domain.Roles;
using Todo.Domain.Users;

namespace Todo.Domain.Tenants;

public class Tenant : Entity
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDayDate { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<Role> Roles { get; set; }
    
}