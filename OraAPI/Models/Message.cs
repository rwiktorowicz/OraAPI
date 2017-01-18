using System;
namespace OraAPI
{
	public class Message
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public User User { get; set; }
	}
}
