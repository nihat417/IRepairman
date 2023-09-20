namespace IRepairman.Domain.Entities
{
    public class Specialization:BaseEntity
    {
        public string Name { get; set; } = null!;
        public virtual IEnumerable<Master>? masters { get; set; }
    }
}
