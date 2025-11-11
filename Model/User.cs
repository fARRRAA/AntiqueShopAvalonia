using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class User
{
    public int Id { get; set; }

    public string? Lname { get; set; }

    public string? Fname { get; set; }

    public string? Patronymic { get; set; }

    public DateOnly? DateBirth { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public virtual Role Role { get; set; } = null!;
}
