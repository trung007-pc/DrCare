namespace Todo.Contract.Users;

public class ResetRequestDto
{
    public string UserName { get; set; }
    public string NewPassword { get; set; }
}