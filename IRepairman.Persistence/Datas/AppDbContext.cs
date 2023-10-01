using IRepairman.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IRepairman.Persistence.Datas
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions options) : base(options) { }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.Entity<Master>()
				.HasOne(m => m.Specialization)   
				.WithMany(s => s.masters)        
				.HasForeignKey(m => m.SpecializationId) 
				.IsRequired();

			builder.Entity<Specialization>().HasData(
				new Specialization { Id = "1" , Name = "Engineer",CreatedDate = DateTime.Now },
                new Specialization { Id = "2", Name = "blacksmith", CreatedDate = DateTime.Now }
                );

			builder.Entity<MessageContact>()
				.HasOne(m=>m.Sender)
				.WithMany()
				.HasForeignKey(m => m.SenderId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.Entity<MessageContact>()
				.HasOne(m => m.Receiver)
				.WithMany()
				.HasForeignKey(m => m.ReceiverId)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(builder);
        }

        public DbSet<AppUser>users { get; set; }
		public DbSet<Master>masters { get; set; }
		public  DbSet<MessageContact> messages { get; set; }
		public DbSet<Specialization> specializations { get; set; }
	}
}
