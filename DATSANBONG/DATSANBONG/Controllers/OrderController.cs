using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DATSANBONG.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController:Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        // Create đơn đặt sân
        [HttpPost("create-order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "KHÁCH HÀNG", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateOrder([FromBody] RequestOrderDTO request)
        {
            var response = await _orderRepository.CreateOrder(request);
            return StatusCode((int)response.Status, response);
        }
    }
}
