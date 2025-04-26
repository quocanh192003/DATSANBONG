using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DATSANBONG.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class ManagementUserController : Controller
    {
        private readonly IConfirmRepository _confirm;
        private readonly APIResponse _apiResponse;
        public ManagementUserController(IConfirmRepository confirm)
        {
            _confirm = confirm;
            this._apiResponse = new APIResponse();
        }

        [HttpPut("confirm_user")]
        public async Task<IActionResult> ConfirmUser([FromBody] ConfirmUserDTO request)
        {
            var user = await _confirm.ConfirmUser(request);
            if (user == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid information or user does not exist!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true; ;
            _apiResponse.Result = user;
            return Ok(_apiResponse);
            
        }
    }
}
