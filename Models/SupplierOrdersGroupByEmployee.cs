using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class SupplierOrdersGroupByEmployee
{
    public int? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public decimal OrderedCost { get; set; }

    public decimal ReturnedCost { get; set; }

    public decimal? TotalCost { get; set; }

    public int? OrdersCount { get; set; }
}
