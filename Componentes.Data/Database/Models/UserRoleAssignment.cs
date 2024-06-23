using System;
using System.Collections.Generic;

namespace Componentes.Data.Models;

public partial class UserRoleAssignment
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual UserRole Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
