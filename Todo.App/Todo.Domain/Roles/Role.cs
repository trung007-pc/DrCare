using Microsoft.AspNetCore.Identity;
using Todo.Domain.BaseEntities;

namespace Todo.Domain.Roles;

public class Role :  IdentityRole<Guid>
{
    public string Code { get; set; }
}