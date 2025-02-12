using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class OrderItem
{
    public string OrderItemId { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? CakeId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual CakeProduct? Cake { get; set; }
}
