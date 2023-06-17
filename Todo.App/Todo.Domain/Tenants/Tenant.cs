using Todo.Domain.BaseEntities;
using Todo.Domain.Roles;
using Todo.Domain.Users;

namespace Todo.Domain.Tenants;

public class Tenant : Entity
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDayDate { get; set; }
    
    
    public List<User> Users { get; set; }
    public List<Role> Roles { get; set; }
}