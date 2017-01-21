using System;
namespace OraAPI
{
	public class ChatReference
	{
		public ChatReference()
		{
		}

		public string Update(int id, string name) { 
			if (!GlobalInfo.LoggedIn)
			{
				throw new Exception("You are not logged in");
			}

			ApiDbContext dbContext = new ApiDbContext();

			Chat updatedChat = new Chat()
			{
				Id = id,
				Name = name
			};

			dbContext.Chats.Attach(updatedChat);
			var entry = dbContext.Entry(updatedChat);
			entry.Property(p => p.Name).IsModified = true;
			dbContext.SaveChanges();

			return "Successfully Updated Chat";
		}
	}
}
