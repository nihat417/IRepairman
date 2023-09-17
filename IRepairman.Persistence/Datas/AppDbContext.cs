using IRepairman.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IRepairman.Persistence.Datas
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions options) : base(options) { }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}

		public DbSet<AppUser>users { get; set; }
		public DbSet<Master>masters { get; set; }
		public DbSet<Specialization> specializations { get; set; }
	}
}
