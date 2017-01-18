using System;
using System.Linq;
using Newtonsoft.Json;
namespace OraAPI
{
	public class Users
	{
		public Users()
		{
		}

		public string Create(string name, string email, string password, string passwordConfirmation)
		{
			if (password != passwordConfirmation)
			{
				throw new Exception("Passwords do not match");
			}

			ApiDbContext dbContext = new ApiDbContext();

			User newUser = new User()
			{
				Name = name,
				Email = email,
				Password = password
			};

			dbContext.Users.Add(newUser);
			dbContext.SaveChanges();

			User addedUser = dbContext.Users.Where(x => x.Email == newUser.Email).SingleOrDefault();
			dbContext.Dispose();

			var returnObject = new
			{
				data = new
				{
					id = addedUser.Id,
					name = addedUser.Name,
					email = addedUser.Email
				},
				meta = new { }
			};

			return JsonConvert.SerializeObject(returnObject);
		}
	}
}
