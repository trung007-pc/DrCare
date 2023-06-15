using Todo.Contract.Roles;

namespace Todo.Contract.Users;

public class UserWithNavigationPropertiesDto
{
    public UserDto User { get; set; }
    public List<RoleDto> Roles { get; set; }
}