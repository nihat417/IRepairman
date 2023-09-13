using IRepairman.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace IRepairman.Domain.Entities
{
	public class AppUser:IdentityUser
	{
		public string FullName { get; set; } = null!;
		public Role Role { get; set; }
		public DateTime CreatedTime { get; set; }
	}
}
