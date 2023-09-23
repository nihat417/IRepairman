using IRepairman.Domain.Entities;

namespace IRepairman.Application.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<Specialization>> GetAllSpecializationsAsync();
        Task<Specialization> GetSpecializationByIdAsync(string id);
        Task<Specialization> GetSpecializationByNameAsync(string name);
        Task UpdateSpecializationAsync(string id);
        Task DeleteSpecializationAsync(string id);
        Task CreateSpecializationAsync(Specialization specialization);
    }
}
