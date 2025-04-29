using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class CustomerOrdersGroupByCustomer
{
    public int? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public decimal OrderedCost { get; set; }

    public decimal ReturnedCost { get; set; }

    public decimal? TotalCost { get; set; }

    public int? OrdersCount { get; set; }
}
