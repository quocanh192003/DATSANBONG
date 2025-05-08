using Azure.Core;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using DATSANBONG.Services;
using DATSANBONG.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO model)
        {
            
            var loginResponse = await _authRepo.Login(model);
            
            if (loginResponse == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Users who do not have access!");
                return BadRequest(_apiResponse);
            }

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterRequestDTO model)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> EmailVerification(RequestEmailVerification request)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ResendEmailVerification(string email)
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

        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<APIResponse>> ChangePassword([FromBody] ChangePasswordDTO request)
        {
            if (!ModelState.IsValid)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid input.");
                return BadRequest(_apiResponse);
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("New password and confirm password do not match.");
                return BadRequest(_apiResponse);
            }

            var userId = User.Identity.Name; // Đây chính là ID user (GUID)
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("User not found.");
                return BadRequest(_apiResponse);
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                foreach (var error in result.Errors)
                {
                    _apiResponse.ErrorMessages.Add(error.Description);
                }
                return BadRequest(_apiResponse);
            }

            _apiResponse.IsSuccess = true;
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.Result = "Password changed successfully.";
            return Ok(_apiResponse);
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Status = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("User not found");
                    return NotFound(_apiResponse);
                }

                // Generate password reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Reset the password
                var result = await _userManager.ResetPasswordAsync(user, token, "Admin@123");
                if (result.Succeeded)
                {
                    _apiResponse.IsSuccess = true;
                    _apiResponse.Status = HttpStatusCode.OK;
                    _apiResponse.Result = "Password reset successfully";
                    return Ok(_apiResponse);
                }

                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add(ex.Message);
                return BadRequest(_apiResponse);
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(RequestForgotPasswordDTO request)
        {
            if (ModelState.IsValid)
            {
                var tokenResponse = await _authRepo.ForgotPassword(request);
                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Status = HttpStatusCode.BadRequest;
                    _apiResponse.Result = "Invalid Input";
                    return BadRequest(_apiResponse);
                }
                _apiResponse.IsSuccess = true;
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.Result = "Please reset password with the code that you received";
                return Ok(_apiResponse);
            }
            _apiResponse.IsSuccess = false;
            _apiResponse.Status = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessages.Add("Something went wrong!");
            return BadRequest(_apiResponse);
        }

        [HttpPost("reset-password-user")]
        public async Task<IActionResult> ResetPasswordUser(RequestResetPasswordUserDTO request)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Status = HttpStatusCode.BadRequest;
                    _apiResponse.Result = "Invalid Input";
                    return BadRequest(_apiResponse);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (result.Succeeded)
                {
                    _apiResponse.IsSuccess = true;
                    _apiResponse.Status = HttpStatusCode.OK;
                    _apiResponse.Result = "Password reset is successfully";
                    return Ok(_apiResponse);
                }
            }
            _apiResponse.IsSuccess = false;
            _apiResponse.Status = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessages.Add("Something went wrong!");
            return BadRequest(_apiResponse);
        }
    }
}
