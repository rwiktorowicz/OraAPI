using System;
using System.Linq;
namespace OraAPI
{
	public class Authentication
	{
		
		public Authentication()
		{
			
		}

		public string Login(string email, string password) {
			ApiDbContext dbContext = new ApiDbContext();
			User user = dbContext.Users.Where(x => x.Email == email && x.Password == password).SingleOrDefault();

			if (user == null)
				throw new Exception("A user with that email/password does not exist");

			GlobalInfo.LoggedInUserId = user.Id;
			GlobalInfo.LoggedInUserName = user.Name;
			GlobalInfo.LoggedIn = true;

			dbContext.Dispose();

			return "Successfully Logged In!";
	}

		public void Logoff() {
			GlobalInfo.LoggedInUserId = 0;
			GlobalInfo.LoggedInUserName = string.Empty;
			GlobalInfo.LoggedIn = false;
}
