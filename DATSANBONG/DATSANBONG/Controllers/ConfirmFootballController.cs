using DATSANBONG.Models;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DATSANBONG.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class ConfirmFootballController : Controller
    {
        private readonly IConfirmFootballRepository _RepoFootball;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly APIResponse _apiResponse;
        public ConfirmFootballController(IConfirmFootballRepository RepoFootball, UserManager<ApplicationUser> userManager)
        {
            _RepoFootball = RepoFootball;
            this._apiResponse = new APIResponse();
            _userManager = userManager;
        }

        //ADMIN CONFIRM FOOTBALL
        [HttpPut("confirm_football/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<APIResponse>> ConfirnFootball(string id, [FromBody] string status)
        {
            try
            {
                var result = await _RepoFootball.confirmFootball(id, status);
                if (result == null)
                {
                    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(status) || status.ToUpper() != "ACTIVE")
                    {
                        _apiResponse.IsSuccess = false;
                        _apiResponse.Status = HttpStatusCode.BadRequest;
                        _apiResponse.ErrorMessages = new List<string> { "Invalid information" };
                        return BadRequest(_apiResponse);
                    }

                    if (await _RepoFootball.GetFootballById(id) == null)
                    {
                        _apiResponse.IsSuccess = false;
                        _apiResponse.Status = HttpStatusCode.NotFound;
                        _apiResponse.ErrorMessages = new List<string> { "FootBall Field does not exists!" };
                        return NotFound(_apiResponse);
                    }


                }
                _apiResponse.IsSuccess = true;
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.Result = result;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }


        [HttpGet("get_all_football")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllFootball()
        {
            try
            {
                var result = await _RepoFootball.GetAllFootball();
                _apiResponse.IsSuccess = true;
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.Result = result;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }

    }
}
