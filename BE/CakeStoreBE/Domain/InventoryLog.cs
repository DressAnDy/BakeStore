using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class InventoryLog
{
    public string InventoryLogId { get; set; } = null!;

    public string? ProductId { get; set; }

    public int? QuantityChange { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product? Product { get; set; }
}
