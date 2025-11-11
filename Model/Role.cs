using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
