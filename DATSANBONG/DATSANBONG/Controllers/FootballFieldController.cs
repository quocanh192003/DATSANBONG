using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace DATSANBONG.Controllers
{
    [Route("api/football")]
    [ApiController]
    public class FootballFieldController : Controller
    {
        private readonly IFootballFieldRepository _football;
        
        public FootballFieldController(IFootballFieldRepository football)
        {
            _football = football;
        }

        //Chủ sân đăng ký thông tin sân bóng
        [HttpPost("create-football")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateFootBall([FromBody] CreateFootballDTO request)
        {
            
            var response = await _football.CreateFootBall(request);
            return StatusCode((int)response.Status, response);
        }

        // Chủ sân cập nhật thông tin sân bóng
        [HttpPut("update-football/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateFootball(string id, [FromBody] UpdateFootballDTO request)
        {

            var response = await _football.UpdateFootball(id, request);
            return StatusCode((int)response.Status, response);
        }

        // Chủ sân đăng thông tin chi tiết sân bóng con
        [HttpPost("create-detailfootball")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateDetailFootball([FromBody] CreateDetailFootballDTO request)
        {

            var response = await _football.CreateDetailFootball(request);
            return StatusCode((int)response.Status, response);
        }

        // nhân viên cập nhật thông tin chi tiết sân bóng con
        [HttpPut("update-detailfootball/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Nhân Viên", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateDetailFootball(string id, [FromBody] UpdateDetailFootballDTO request)
        {

            var response = await _football.UpdateDetailFootball(id, request);
            return StatusCode((int)response.Status, response);
        }

    }
}
