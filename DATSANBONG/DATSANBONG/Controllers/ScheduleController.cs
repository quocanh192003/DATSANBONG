using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DATSANBONG.Controllers
{
    [Route("api")]
    [ApiController]
    public class ScheduleController: ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        // Create lịch sân
        [HttpPost("create-schedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "CHỦ SÂN, NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateSchedule([FromBody] LichSanDTO request)
        {
            var response = await _scheduleRepository.CreatSchedule(request);
            return StatusCode((int)response.Status, response);
        }

        // Get all lịch sân
        [HttpGet("get-all-schedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllSchedule()
        {
            var response = await _scheduleRepository.GetAllSchedule();
            return StatusCode((int)response.Status, response);
        }

        // Get lịch sân by id detail football
        [HttpGet("get-schedule-by-id-detail-football/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetScheduleByIdDetailFootball(string id)
        {
            var response = await _scheduleRepository.GetScheduleByIdDetailFootball(id);
            return StatusCode((int)response.Status, response);
        }

        // Update lịch sân
        [HttpPut("update-schedule/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "CHỦ SÂN, NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpdateScheduleDTO request)
        {
            var response = await _scheduleRepository.UpdateSchedule(id, request);
            return StatusCode((int)response.Status, response);
        }

        // Delete lịch sân
        [HttpDelete("delete-schedule/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "CHỦ SÂN, NHÂN VIÊN", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var response = await _scheduleRepository.DeleteSchedule(id);
            return StatusCode((int)response.Status, response);
        }



    }
}
