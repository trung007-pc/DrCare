using Todo.Core.Enums;

namespace Todo.Contract.Users;

public class CreateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    
    public string? Email { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    
    public string PhoneNumber { get; set; }

    public DateTime DOB { get; set; } = DateTime.Now;
    
    public bool IsActive { get; set; } 
    public List<Guid> RoleIds { get; set; } = new List<Guid>();
}