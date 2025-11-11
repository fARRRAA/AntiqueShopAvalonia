using System;
using System.Collections.Generic;

namespace AntiqueShopAvalonia.Model;

public partial class Return
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public string? Reason { get; set; }

    public int? ReturnTypeId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ReturnType? ReturnType { get; set; }
}
