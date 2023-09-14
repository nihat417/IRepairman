namespace IRepairman.Domain.Entities
{
    public class Master:AppUser
    {
        public virtual List<Specialization> Specializations { get; set; } = null!;
        public string? WorkExperience { get; set; }
    }
}
