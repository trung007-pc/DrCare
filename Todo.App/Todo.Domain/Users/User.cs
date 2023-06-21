using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Serializers;
using Todo.Core.Enums;
using Todo.Domain.BaseEntities;
using Todo.Domain.Tenants;

namespace Todo.Domain.Users;

public class User : IdentityUser<Guid> 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName
    {
        get { return FirstName + LastName; }
    }

    public Gender Gender { get; set; }

    public DateTime LastModificationTime { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime DeletionTime { get; set; } = DateTime.Now;
    public Guid CreatorId { get; set; }
    public string? AvatarURL { get; set; }
    public DateTime DOB { get; set; }
    
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; }

    public List<Guid>? TenantIds { get; set; }


}