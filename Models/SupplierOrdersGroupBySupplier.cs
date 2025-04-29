using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class SupplierOrdersGroupBySupplier
{
    public int? SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public decimal OrderedCost { get; set; }

    public decimal ReturnedCost { get; set; }

    public decimal? TotalCost { get; set; }

    public int? OrdersCount { get; set; }
}
