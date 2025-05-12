using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class CustomerOrder
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTimeOffset Date { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<CustomerOrderItem> CustomerOrderItems { get; set; } = new List<CustomerOrderItem>();

    public virtual Employee? Employee { get; set; }

    public string LocalDate
    {
        get
        {
            return Date.DateTime.ToShortDateString() + " " + Date.DateTime.ToShortTimeString();
        }
    }

    public decimal TotalCost => CustomerOrderItems.Sum(Entry => Entry.TotalCost);
}
