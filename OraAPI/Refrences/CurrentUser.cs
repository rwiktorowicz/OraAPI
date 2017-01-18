using System;
using System.Linq;
using Newtonsoft.Json;

namespace OraAPI
{
	public class CurrentUser
	{
		public CurrentUser()
		{
		}

		public string Read() {
			if (!GlobalInfo.LoggedIn)
			{
				throw new Exception("You are not logged in");
			}
			ApiDbContext dbContext = new ApiDbContext();

			User user = dbContext.Users.Where(x => x.Id == GlobalInfo.LoggedInUserId).SingleOrDefault();

			var returnObject = new
			{
				data = new
				{
					id = user.Id,
					name = user.Name,
					email = user.Email
				},
				meta = new { }
			};

			return JsonConvert.SerializeObject(returnObject);
	}

		public string Update(string name, string email, string password, string passwordConfirmation) {
			if (!GlobalInfo.LoggedIn)
			{
				throw new Exception("You are not logged in");
			}

			if (password != passwordConfirmation)
			{
				throw new Exception("Passwords do not match");
			}
			ApiDbContext dbContext = new ApiDbContext();

			User updatedUser = new User()
			{
				Name = name,
				Email = email
			};

			dbContext.Users.Attach(updatedUser);
			var entry = dbContext.Entry(updatedUser);
			entry.Property(p => p.Name).IsModified = true;
			entry.Property(p => p.Email).IsModified = true;
			dbContext.SaveChanges();

			User user = dbContext.Users.Where(x => x.Name == name && x.Email == email).SingleOrDefault();

			dbContext.Dispose();

			var returnObject = new
			{
				data = new
				{
					id = user.Id,
					name = user.Name,
					email = user.Email
				},
				meta = new { }
			};

			return JsonConvert.SerializeObject(returnObject);
}
