namespace IRepairman.Domain.Entities
{
    public class Master:AppUser
    {
        public virtual List<Specialization> Specializations { get; set; } = null!;
        public int WorkExperience { get; set; }
        public virtual List<int> SelectedSpecializations { get; set; } = null!;
        public string? About { get; set; }
    }
}
