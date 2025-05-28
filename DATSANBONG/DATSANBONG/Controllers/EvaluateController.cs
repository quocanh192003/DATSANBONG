using DATSANBONG.Models.DTO;
using DATSANBONG.Repository;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DATSANBONG.Controllers
{
    [Route("api")]
    [ApiController]
    public class EvaluateController : Controller
    {
        private readonly IEvaluateRepository _evaluateRepository;
        public EvaluateController(IEvaluateRepository evaluateRepository)
        {
            _evaluateRepository = evaluateRepository;
        }

        // Khách hàng tạo đánh giá sân bóng
        [HttpPost("create-evaluate/{maSanBong}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "KHÁCH HÀNG", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateOrder(string maSanBong,[FromBody] ResquestCreateEvaluateDTO request)
        {
            var response = await _evaluateRepository.createEvaluate(maSanBong, request);
            return StatusCode((int)response.Status, response);
        }

        // Tính số sao trung bình của một sân bóng
        //[HttpPost("average-evaluate/{maSanBong}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> AverageEvaluate(string maSanBong)
        //{
        //    var response = await _evaluateRepository.AverageEvaluate(maSanBong);
        //    return StatusCode((int)response.Status, response);
        //}

        // Lấy ra tất cả bình luận của một sân bóng
        [HttpGet("get-evaluate-by-id-san-bong/{maSanBong}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEvaluateByIdSanBong(string maSanBong)
        {
            var response = await _evaluateRepository.getEvaluateByIdSanBong(maSanBong);
            return StatusCode((int)response.Status, response);
        }

        // Xóa bình luận theo mã đánh giá
        [HttpDelete("delete-evaluate-by-masanbong/{maDanhGia}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "KHÁCH HÀNG", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> deleteEvaluate(string maDanhGia)
        {
            var response = await _evaluateRepository.deleteEvaluate(maDanhGia);
            return StatusCode((int)response.Status, response);
        }

        // Update bình luận theo mã đánh giá
        [HttpPut("update-evaluate/{maDanhGia}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "KHÁCH HÀNG", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateEvaluate(string maDanhGia, [FromBody] UpdateEvaluateDTO request)
        {
            var response = await _evaluateRepository.updateEvaluate(maDanhGia, request);
            return StatusCode((int)response.Status, response);
        }
    }
}
