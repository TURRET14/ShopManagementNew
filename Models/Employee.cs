using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? Experience { get; set; }

    public string? Position { get; set; }

    public string? UserLogin { get; set; }

    public string? UserPassword { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    public virtual ICollection<CustomerReturnItem> CustomerReturnItems { get; set; } = new List<CustomerReturnItem>();

    public virtual ICollection<SupplierOrder> SupplierOrders { get; set; } = new List<SupplierOrder>();

    public virtual ICollection<SupplierReturnItem> SupplierReturnItems { get; set; } = new List<SupplierReturnItem>();

    public string NameId => $"{Name} ID_{Id}";
}
