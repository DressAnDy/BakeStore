using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Discount
{
    public string DiscountId { get; set; } = null!;

    public string? ProductId { get; set; }

    public decimal? DiscountAmount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product? Product { get; set; }
}
