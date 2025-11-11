using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public decimal? ShopShare { get; set; }

    public int? ClientId { get; set; }

    public DateOnly? ReceiptDate { get; set; }

    public int? StoragePeriod { get; set; }

    public int? StatusId { get; set; }

    public virtual ProductType? Category { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ProductStatus? Status { get; set; }
}
