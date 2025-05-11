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
        [Authorize(Roles = "CHỦ SÂN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateFootBall([FromForm] CreateFootballDTO request)
        {
            
            var response = await _football.CreateFootBall(request);
            return StatusCode((int)response.Status, response);
        }

        // Chủ sân cập nhật thông tin sân bóng
        [HttpPut("update-football/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "CHỦ SÂN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateFootball(string id, [FromForm] UpdateFootballDTO request)
        {

            var response = await _football.UpdateFootball(id, request);
            return StatusCode((int)response.Status, response);
        }

        // chủ sân xóa hình ảnh sân bóng
        // Chủ sân cập nhật thông tin sân bóng
        [HttpDelete("remove-image-football/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "CHỦ SÂN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveImageFootball(string id)
        {
            var response = await _football.DeleteImage(id);
            return StatusCode((int)response.Status, response);
        }

        // Chủ sân đăng thông tin chi tiết sân bóng con
        [HttpPost("create-detailfootball")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "CHỦ SÂN", AuthenticationSchemes = "Bearer")]
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
        [Authorize(Roles = "NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateDetailFootball(string id, [FromBody] UpdateDetailFootballDTO request)
        {

            var response = await _football.UpdateDetailFootball(id, request);
            return StatusCode((int)response.Status, response);
        }

        //Lấy danh sách sân bóng
        [HttpGet("get-all-football")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllFootballField()
        {
            var response = await _football.GetAllFootballField();
            return StatusCode((int)response.Status, response);
        }

        //Lấy thông tin sân bóng theo id
        [HttpGet("get-football/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFootballFieldById(string id)
        {
            var response = await _football.GetFootballFieldById(id);
            return StatusCode((int)response.Status, response);
        }

        //Lấy thông tin chi tiết sân bóng con theo id
        [HttpGet("get-detailfootball/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDetailFootballFieldById(string id)
        {
            var response = await _football.GetDetailFootballFieldById(id);
            return StatusCode((int)response.Status, response);
        }

        //Lấy danh sách sân bóng con
        [HttpGet("get-all-detailfootball")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllDetailFootballField()
        {
            var response = await _football.GetAllDetailFootballField();
            return StatusCode((int)response.Status, response);
        }

        //Lấy danh sách sân bóng con theo trạng thái sân trống
        [HttpGet("{id}/lich-san-trong")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDetailFootballbyStatus(string id)
        {
            var response = await _football.GetDetailFootballbyStatus(id);
            return StatusCode((int)response.Status, response);
        }

    }
}
