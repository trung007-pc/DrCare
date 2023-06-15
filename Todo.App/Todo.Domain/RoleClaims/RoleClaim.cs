﻿using Microsoft.AspNetCore.Identity;
using Todo.Domain.BaseEntities;

namespace Todo.Domain.RoleClaims;

public class RoleClaim :  IdentityRoleClaim<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}