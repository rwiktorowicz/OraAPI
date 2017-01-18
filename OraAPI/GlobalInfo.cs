using System;
namespace OraAPI
{
	public static class GlobalInfo
	{
		public static int LoggedInUserId { get; set; }
		public static string LoggedInUserName { get; set; }
		public static bool LoggedIn { get; set; }
	}
}
