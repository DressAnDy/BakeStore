using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Category
{
    public string CategoryId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CakeProduct> Cake { get; set; } = new List<CakeProduct>();
}
