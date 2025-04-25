using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DATSANBONG.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepositoty _authRepo;
        private readonly APIResponse _apiResponse;
        public AuthController(IAuthRepositoty authRepo)
        {
            _authRepo = authRepo;
            this._apiResponse = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _authRepo.Login(model);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Username or password is incorrect!");
                return BadRequest(_apiResponse);
            }

            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true; ;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            bool ifUserNameUnique = _authRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Usernam already exists!");
                return BadRequest(_apiResponse);
            }

            var user = await _authRepo.Register(model);
            if (user == null)
            {
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Error while registering!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
