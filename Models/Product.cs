using System;
using System.Collections.Generic;

namespace ShopManagement.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Amount { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CustomerOrderItem> CustomerOrderItems { get; set; } = new List<CustomerOrderItem>();

    public virtual ICollection<SupplierOrderItem> SupplierOrderItems { get; set; } = new List<SupplierOrderItem>();

    public string NameId => $"{Name} ID_{Id}";

    public string NameIdPriceAmount => $"{Name} ID_{Id} | Цена - {Price} | В Наличии - {Amount}";
}
