using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class CustomerOrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<CustomerReturnItem> CustomerReturnItems { get; set; } = new List<CustomerReturnItem>();

    public virtual CustomerOrder Order { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public decimal TotalCost => Price * Amount;
}
