using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Shipping
{
    public string ShippingId { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? ShippingMethod { get; set; }

    public string? ShippingStatus { get; set; }

    public DateTime? ShippingDate { get; set; }

    public DateTime? EstimatedArrival { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }
}
