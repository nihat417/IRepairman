namespace IRepairman.Domain.Entities
{
	public class MessageContact:BaseEntity
	{
		public string SenderId { get; set; }
		public string ReceiverId { get; set; }
		public string Content { get; set; }
		public DateTime SentAt { get; set; }

		public virtual AppUser Sender { get; set; }
		public virtual AppUser Receiver { get; set; }
	}
}
