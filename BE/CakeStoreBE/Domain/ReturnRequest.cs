using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class ReturnRequest
{
    public string ReturnRequestId { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? Reason { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }
}
