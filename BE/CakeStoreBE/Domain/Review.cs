using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Review
{
    public string ReviewId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? CakeId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CakeProduct? Cake { get; set; }

    public virtual User? User { get; set; }
}
