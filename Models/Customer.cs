using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
    
    public string NameId => $"{Name} ID_{Id}";
}
