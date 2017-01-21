using System;
namespace OraAPI
{
	public class Message
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public DateTime CreationDate { get; set; }

		public User User { get; set; }
		public Chat Chat { get; set; }
	}
}
