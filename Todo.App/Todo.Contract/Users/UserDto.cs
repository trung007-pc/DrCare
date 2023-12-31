﻿using Todo.Core.Enums;

namespace Todo.Contract.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName
    {
        get { return FirstName + LastName; }
    }

    public Gender Gender { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; } 
    public DateTime DOB { get; set; }
    
    public bool IsActive { get; set; }
}