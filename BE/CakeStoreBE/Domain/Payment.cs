using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Payment
{
    public string PaymentId { get; set; } = null!;

    public string? OrderId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }
}
