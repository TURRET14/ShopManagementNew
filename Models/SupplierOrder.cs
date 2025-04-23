using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class SupplierOrder
{
    public int Id { get; set; }

    public int? SupplierId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTimeOffset Date { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<SupplierOrderItem> SupplierOrderItems { get; set; } = new List<SupplierOrderItem>();

    public string LocalDate
    {
        get
        {
            return Date.DateTime.ToShortDateString() + " " + Date.DateTime.ToShortTimeString();
        }
    }
}
