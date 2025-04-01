using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class CustomerReturnItem
{
    public int Id { get; set; }

    public int OrderItemId { get; set; }

    public int Amount { get; set; }

    public int? EmployeeId { get; set; }

    public string? Reason { get; set; }

    public DateOnly Date { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual CustomerOrderItem OrderItem { get; set; } = null!;
}
