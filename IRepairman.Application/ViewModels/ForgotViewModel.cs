using System.ComponentModel.DataAnnotations;

namespace IRepairman.Application.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}
