using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class ProductStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
