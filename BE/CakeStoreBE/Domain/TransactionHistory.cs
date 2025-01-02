using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class TransactionHistory
{
    public string TransactionId { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? TransactionType { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }
}
