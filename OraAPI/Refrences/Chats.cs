using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OraAPI
{
	public class Chats
	{
		public Chats()
		{
		}

		public string List(int page, int limit)
		{
			ApiDbContext dbContext = new ApiDbContext();
			IEnumerable<Chat> allChats = dbContext.Chats;
			List<object> dataList = new List<object>();

			foreach (Chat chat in allChats)
			{
				// Create User List
				List<object> userList = new List<object>();

				foreach (Message message in chat.Messages)
				{
					var user = new
					{
						id = message.User.Id,
						name = message.User.Name,
						email = message.User.Email
					};

					userList.Add(user);
				}

				//Create Last Chat Message
				Message lastMessage = chat.Messages.OrderByDescending(o => o.CreationDate).FirstOrDefault();

				var lastMessageMeta = new
				{
					id = lastMessage.Id,
					chat_id = chat.Id,
					user_id = lastMessage.User.Id,
					viewed_at = DateTime.Now.ToString(),
					message = lastMessage.Message,
					created_at = lastMessage.CreationDate.ToString(),
					user = new { 
						id = lastMessage.User.Id,
						name = lastMessage.User.Name,
						email = lastMessage.User.Email
					}

				};

				var data = new
				{
					id = chat.Id,
					name = chat.Name,
					user_id = chat.User.Id,
					users = userList.ToArray(),
					last_chat_message = lastMessageMeta
				};

				dataList.Add(data);

			}

			var returnData = new
			{
				data = dataList.ToArray(),
				meta = new
				{
					pagination = new
					{
						current_page = page,
						per_page = limit,
						page_count = Math.Floor((double)allChats.Count()/limit),
						total_count = allChats.Count()
					}
				}
			};

			dbContext.Dispose();

			return JsonConvert.SerializeObject(returnData);
		}

		public string Create(string name, string message) {
			if (!GlobalInfo.LoggedIn) {
				throw new Exception("Not logged in");
			}

			ApiDbContext dbContext = new ApiDbContext();

			Chat newChat = new Chat()
			{
				Name = name,
				User = dbContext.Users.Where(x => x.Id == GlobalInfo.LoggedInUserId).SingleOrDefault()
			};

			Message newMessage = new Message()
			{
				Message = message,
				CreationDate = DateTime.Now,
				User = dbContext.Users.Where(x => x.Id == GlobalInfo.LoggedInUserId).SingleOrDefault()
			};

			dbContext.Chats.Add(newChat);
			dbContext.SaveChanges();

			Chat addedChat = dbContext.Chats.Where(x => x.Name == newChat.Name && x.User == newChat.User).SingleOrDefault();
			newMessage.Chat = addedChat;

			dbContext.Messages.Add(newMessage);
			dbContext.SaveChanges();

			Message addedMessage = dbContext.Messages.Where(x => x.Message == newMessage.Message && x.User == newMessage.User && x.Chat == newMessage.Chat).SingleOrDefault();



			List<object> userList = new List<object>();

			foreach (Message chatMessage in addedChat.Messages)
			{
				var userMeta = new
				{
					id = chatMessage.User.Id,
					name = chatMessage.User.Name,
					email = chatMessage.User.Email
				};

				userList.Add(userMeta);
			}

			dbContext.Dispose();

			var returnObject = new
			{
				data = new
				{
					id = addedChat.Id,
					name = addedChat.Name,
					user_id = addedChat.User.Id,
					users = userList.ToArray(),
					last_chat_message = new
					{
						id = addedMessage.Id,
						chat_id = addedMessage.Chat.Id,
						user_id = addedMessage.User.Id,
						message = addedMessage.Message,
						viewed_at = DateTime.Now.ToString(),
						created_at = addedMessage.CreationDate.ToString(),
						user = new
						{
							id = addedMessage.User.Id,
							name = addedMessage.User.Name,
							email = addedMessage.User.Email
						}
					}
				},

				meta = new { }
			};

			return JsonConvert.SerializeObject(returnObject);
	}
}
