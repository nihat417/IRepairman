namespace IRepairman.Domain.Entities
{
    public class Master:AppUser
    {
        public int WorkExperience { get; set; }
        public string? About { get; set; }
        public string? SpecializationId { get; set; } = null!;
        //public virtual Specialization? Specialization { get; set; }
        public virtual List<Specialization>? Specializations { get; set; }
    }
}
