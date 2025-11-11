using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
