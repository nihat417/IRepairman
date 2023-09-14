using System.ComponentModel.DataAnnotations;

namespace IRepairman.Application.ViewModels
{
    public class UserRegisterVM
    {
        [Required]
        [MinLength(5)]
        public string FullName { get; set; } = null!;
        [Required]
        [MinLength(4)]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Range(18, 99)]
        public int Age { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
