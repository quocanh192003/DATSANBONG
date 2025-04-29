using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models.DTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Email { get; set; }
        
        //[Required]
        //public string NewPassword { get; set; }
        
        //[Required]
        //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        //public string ConfirmNewPassword { get; set; }
    }
}