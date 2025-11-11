using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class Sale
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? ClientId { get; set; }

    public DateOnly? SaleDate { get; set; }

    public int? PayMethodId { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? ShopAmount { get; set; }

    public decimal? ClientAmount { get; set; }

    public virtual Client? Client { get; set; }

    public virtual PaymentMethod? PayMethod { get; set; }

    public virtual Product? Product { get; set; }
}
