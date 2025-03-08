using CakeStoreBE.Application.Services;
using CakeStoreBE.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CakeStoreBE.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

    [HttpGet("Get All Order")]
        public async Task<IActionResult> GetAllOrder()
        {
            var get_all_order = await _orderServices.HandleGetAllOrderAsync();
            return Ok(get_all_order);
        }

        [HttpGet("{orderId}" + "Get Order By Id")]
        public async Task<IActionResult> GetOrderById(string orderId){
            var get_order_by_id = await _orderServices.HandleGetOrderByIdAsync(orderId);
            return Ok(get_order_by_id);
        }

        [HttpPost("Create Order")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order){
            var create_order = await _orderServices.HandleCreateOrderAsync(order);
            return Ok(create_order);
        }

        [HttpPost("{orderId}" + "Update Order")]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order){
            var update_order = await _orderServices.HandleUpdateOrderAsync(order);
            return Ok(update_order);
        }

        [HttpDelete("{orderId}" + "Delete Order")] 
        public async Task<IActionResult> DeleteOrder(string orderId){
            var delete_order = await _orderServices.HandleDeleteOrderAsync(orderId);
            return Ok(delete_order);
        }
    }
}