using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class SupplierReturnItem
{
    public int Id { get; set; }

    public int OrderItemId { get; set; }

    public int Amount { get; set; }

    public int? EmployeeId { get; set; }

    public string? Reason { get; set; }

    public DateTimeOffset Date { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual SupplierOrderItem OrderItem { get; set; } = null!;

    public string LocalDate
    {
        get
        {
            return Date.DateTime.ToShortDateString() + " " + Date.DateTime.ToShortTimeString();
        }
    }

    public decimal TotalCost
    {
        get
        {
            if (OrderItem == null)
            {
                return 0;
            }
            else
            {
                return OrderItem.Price * Amount;
            }
        }
    }
}
