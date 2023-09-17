using IRepairman.Application.Interfaces;
using IRepairman.Domain.Entities;
using IRepairman.Persistence.Datas;
using Microsoft.EntityFrameworkCore;

namespace IRepairman.Persistence.Repository
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly AppDbContext context;

        public SpecializationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateSpecializationAsync(Specialization specialization)
        {
            if(specialization != null)
            {
                await context.specializations.AddAsync(specialization);
                await context.SaveChangesAsync();
            }
            throw new ArgumentNullException(nameof(specialization));
        }

        public async Task DeleteSpecializationAsync(string id)
        {
            var spec = await context.specializations.FindAsync(id);
            if(spec != null)
            {
                context.specializations.Remove(spec);
                await context.SaveChangesAsync();
            }
            throw new ArgumentNullException();
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            var spec = await context.specializations.ToListAsync();
            return spec;
        }

        public async Task<Specialization> GetSpecializationByIdAsync(string id)
        {
            var spec = await context.specializations.FindAsync(id);
            if(spec != null)
            {
                return spec;
            }
            throw new NotImplementedException();
        }

        public async Task<Specialization> GetSpecializationByNameAsync(string name)
        {
            return await context.specializations.FindAsync(name);
        }

        public async Task UpdateSpecializationAsync(Specialization specialization)
        {
            if(specialization != null)
            {
                context.specializations.Update(specialization);
                await context.SaveChangesAsync();
            }
            throw new NotImplementedException();
        }
    }

}
