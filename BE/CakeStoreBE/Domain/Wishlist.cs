using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Wishlist
{
    public string WishlistId { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }
}
