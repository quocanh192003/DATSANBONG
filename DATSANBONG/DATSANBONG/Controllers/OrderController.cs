﻿using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DATSANBONG.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : Controller
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
        [Authorize(Roles = "KHÁCH HÀNG, CHỦ SÂN, NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateOrder([FromBody] RequestOrderDTO request)
        {
            var response = await _orderRepository.CreateOrder(request);
            return StatusCode((int)response.Status, response);
        }

        // Lấy ra danh sách đơn đặt sân theo trạng thái
        [HttpGet("get-all-order-by-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "CHỦ SÂN, NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllOrderbyStatus(string status)
        {
            var response = await _orderRepository.GetAllOrderbyStatus(status);
            return StatusCode((int)response.Status, response);
        }

        // Lấy ra danh sách đơn đặt sân theo trạng thái thanh toán
        [HttpGet("get-all-order-by-status-tt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "CHỦ SÂN, NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllOrderbyStatusTT(string status)
        {
            var response = await _orderRepository.GetAllOrderbyStatusTT(status);
            return StatusCode((int)response.Status, response);
        }

        // Nhân viên xác nhân thanh toán
        [HttpPut("confirm-payment/{idOrder}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ConfirmPayment(string idOrder)
        {
            var response = await _orderRepository.ConfirmPayment(idOrder);
            return StatusCode((int)response.Status, response);
        }

        // Nhân viên xác nhận đơn đặt sân + Xác nhận hủy sân
        [HttpPut("confirm-order/{idOrder}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ConfirmOrder(string idOrder, [FromBody] ConfirmOrderStatusDTO status)
        {
            var response = await _orderRepository.ConfirmOrder(idOrder, status);
            return StatusCode((int)response.Status, response);
        }

        // Khách hàng yêu cầu hủy đặt sân
        [HttpPut("cancel-order/{idOrder}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "KHÁCH HÀNG", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CancelOrder(string idOrder, [FromBody] ConfirmOrderStatusDTO status)
        {
            var response = await _orderRepository.ConfirmOrder(idOrder, status);
            return StatusCode((int)response.Status, response);
        }

        // Lấy ra danh sách đơn đặt sân của người dùng
        [HttpGet("get-order-by-id-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "KHÁCH HÀNG", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderByIdUser()
        {
            var response = await _orderRepository.GetOrderByIdUser();
            return StatusCode((int)response.Status, response);
        }

        // Lấy ra danh sách đơn đặt sân của nhân viên
        [HttpGet("get-order-by-id-staff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderByIdStaff()
        {
            var response = await _orderRepository.GetOrderByIdStaff();
            return StatusCode((int)response.Status, response);
        }
    }
}
