using System;
using System.Data.Entity;
namespace OraAPI
{
	public class ApiDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Chat> Chats { get; set; }
		public DbSet<Message> Messages { get; set; }
	}
}
