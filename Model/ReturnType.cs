using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class ReturnType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();
}
