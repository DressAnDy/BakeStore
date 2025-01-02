using System;
using System.Collections.Generic;

namespace CakeStoreBE.Domain;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? OrderStatus { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();

    public virtual User? User { get; set; }
}
