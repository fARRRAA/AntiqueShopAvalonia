using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class Client
{
    public int Id { get; set; }

    public string? Lname { get; set; }

    public string? Fname { get; set; }

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string? PassportData { get; set; }

    public string? BankDetail { get; set; }

    public string? Email { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
