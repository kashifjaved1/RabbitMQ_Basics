using Microsoft.AspNetCore.Mvc;
using Producer.Dtos;
using Producer.Services.Contracts;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var result = await _orderService.SaveOrder(orderDto);

            if(result < 300)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var result = await _orderService.GetAllOrders();

            if (result is not null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
