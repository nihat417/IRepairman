using System.ComponentModel.DataAnnotations;

namespace IRepairman.Domain.Enums
{
	public enum Role
	{
		[Display( Name = "User")]
		User = 0,
		[Display(Name = "Master")]
		Master = 1,
		[Display(Name = "Admin")]
		Admin = 2,
	}
}
