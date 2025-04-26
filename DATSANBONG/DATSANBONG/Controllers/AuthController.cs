using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using DATSANBONG.Services;
using DATSANBONG.Services.IServices;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public AuthController(IAuthRepositoty authRepo, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _authRepo = authRepo;
            this._apiResponse = new();
            _userManager = userManager;
            _emailService = emailService;
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

            try
            {
                var user = await _authRepo.Register(model);
                if (user == null)
                {
                    _apiResponse.Status = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessages.Add("Error while registering!");
                    return BadRequest(_apiResponse);
                }
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.Result = user;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add(ex.Message);
                return BadRequest(_apiResponse);
            }
        }


        [HttpPost("email-verification")]
        public async Task<IActionResult> EmailVerification(RequestEmailVerification request)
        {
            if (request.Email == null || request.Code == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid Input");
                return BadRequest(_apiResponse);
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid Input");
                return BadRequest(_apiResponse);
            }
            var isVerified = await _userManager.ConfirmEmailAsync(user, request.Code);
            if (isVerified.Succeeded)
            {
                _apiResponse.IsSuccess = true;
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.Result = "Email Verified Successfully";
                return Ok(_apiResponse);
            }
            _apiResponse.IsSuccess = false;
            _apiResponse.Status = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessages.Add("Something went wrong!");
            return BadRequest(_apiResponse);

        }

        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> ResendEmailVerification(string email)
        {
            if (email == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid Input");
                return BadRequest(_apiResponse);
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid Input");
                return BadRequest(_apiResponse);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmail(user.Email, "Email Confirmation", code);

            _apiResponse.IsSuccess = true;
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.Result = "Verification email sent successfully";
            return Ok(_apiResponse);
        }
    }
}
