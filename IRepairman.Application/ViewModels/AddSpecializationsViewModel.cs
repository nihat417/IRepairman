using System.ComponentModel.DataAnnotations;

namespace IRepairman.Application.ViewModels
{
    public class AddSpecializationsViewModel
    {
        public string? id { get;set; }
        [Required]
        [MinLength(4)]
        public string Name { get; set; } = null!;
    }
}
