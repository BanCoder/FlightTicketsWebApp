using FlightTicketsWeb.Core.Entities;

namespace FlightTicketsWeb.Core.Interfaces
{
	public interface IAuthService
	{
		Task<SystemUser?> AuthenticateAsync(string email, string password); 
		Task<SystemUser> RegisterAsync(string email, string password, string firstName, string LastName, string role="user");
		Task SignInAsync(HttpContext httpContext, SystemUser user);
		Task SignOutAsync(HttpContext httpContext);
		SystemUser? GetCurrentUser(HttpContext httpContext);
		bool IsAdmin(SystemUser user); 
		bool IsUser(SystemUser user);
	}
}
