using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class SupplierOrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public virtual SupplierOrder Order { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual ICollection<SupplierReturnItem> SupplierReturnItems { get; set; } = new List<SupplierReturnItem>();

    public decimal TotalCost => Price * Amount;
}
