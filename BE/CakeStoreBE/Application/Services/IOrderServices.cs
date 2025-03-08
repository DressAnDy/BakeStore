using CakeStoreBE.Domain;
using CakeStoreBE.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeStoreBE.Application.Services
{
    public interface IOrderServices
    {
        public Task<Order> HandleCreateOrderAsync(Order order);
        public Task<Order?> HandleGetOrderByIdAsync(string orderId);
        public Task<List<Order>> HandleGetAllOrderAsync();
        public Task<bool> HandleUpdateOrderAsync(Order updateOrderDTO);
        public Task<bool> HandleDeleteOrderAsync(string orderId);  
    }

    public class OrderServices : IOrderServices
    {
            private readonly BakeStoreDbContext _context;

            public OrderServices(BakeStoreDbContext context)
            {
                _context = context;
            }
        public async Task<Order?> HandleGetOrderByIdAsync(string orderId)
        {
            return await _context.Orders
                        .Include(o => o.OrderItems)
                        .Include(o => o.User)
                        .Include(o => o.Payments)
                        .Include(o => o.ReturnRequests)
                        .Include(o => o.Shippings)
                        .Include(o => o.TransactionHistories)
                        .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> HandleGetAllOrderAsync()
        {
            return await _context.Orders
                        .Include(o => o.OrderItems)
                        .Include(o => o.User)
                        .Include(o => o.Payments)
                        .Include(o => o.ReturnRequests)
                        .Include(o => o.Shippings)
                        .Include(o => o.TransactionHistories)
                        .ToListAsync();
        }

        //tạo đơn hàng
        public async Task<Order> HandleCreateOrderAsync(Order order){
            order.CreatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> HandleUpdateOrderAsync(Order updateOrderDTO){
            var existingOrder = await _context.Orders.FindAsync(updateOrderDTO.OrderId);

            if(existingOrder == null) return false;

            existingOrder.OrderStatus = updateOrderDTO.OrderStatus;
            existingOrder.TotalPrice = updateOrderDTO.TotalPrice;        
            existingOrder.UpdatedAt = DateTime.UtcNow;


            _context.Orders.Update(existingOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HandleDeleteOrderAsync(string orderId){
            var existingOrder = await _context.Orders.FindAsync(orderId);
            if(existingOrder == null) return false;
            _context.Orders.Remove(existingOrder);
            await _context.SaveChangesAsync();
            return true;
            }
        }
}