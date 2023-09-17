using System.ComponentModel.DataAnnotations;

namespace IRepairman.Application.ViewModels
{
    public class AddSpecializationsViewModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; } = null!;
    }
}
