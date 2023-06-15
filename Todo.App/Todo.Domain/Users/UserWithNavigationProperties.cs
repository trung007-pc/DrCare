using Todo.Domain.Roles;

namespace Todo.Domain.Users;

public class UserWithNavigationProperties
{
    public User User { get; set; }
     public List<Role> Roles { get; set; }
}