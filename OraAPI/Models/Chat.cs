using System;
using System.Collections.Generic;

namespace OraAPI
{
	public class Chat
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public User User { get; set; }
		public List<Message> Messages { get; set; }
	}
}
