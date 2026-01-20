using BCrypt.Net;
using FlightTicketsWeb.Core.Entities;
using FlightTicketsWeb.Core.Interfaces;
using FlightTicketsWeb.Web.ViewModels.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlightTicketsWeb.Infrastructure.Services
{
	public class AuthService: IAuthService
	{
		private readonly FlightTicketsContext _context;
		public AuthService(FlightTicketsContext context)
		{
			_context = context;	
		}
		public async Task<SystemUser?> AuthenticateAsync(string email, string password)
		{
			var user = await _context.SystemUsers.FirstOrDefaultAsync(u => u.Email == email); 
			if(user == null)
			{
				return null;
			}
			if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
			{
				return null;
			}
			return user;
		}
		public async Task<SystemUser> RegisterAsync(string email, string password, string firstName, string lastName, string role = "user")
		{
			var existingUser = await _context.SystemUsers.FirstOrDefaultAsync(u => u.Email == email); 
			if(existingUser != null)
			{
				throw new Exception("Пользователь с таким email уже существует!"); 
			}
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
			var user = new SystemUser
			{
				Email = email,
				PasswordHash = passwordHash, 
				FirstName = firstName,
				LastName = lastName, 
				Role = role, 
				CreatedAt = DateTime.Now
			};  
			_context.SystemUsers.Add(user);
			await _context.SaveChangesAsync();
			return user; 
		}
		public async Task SignInAsync(HttpContext httpContext, SystemUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
				new Claim(ClaimTypes.Role, user.Role),
				new Claim("UserId", user.Id.ToString()),
				new Claim("FirstName", user.FirstName ?? ""),
				new Claim("LastName", user.LastName ?? "")
			}; 
			var claimIdentity = new ClaimsIdentity(claims, "Cookies");
			var claimPrincipal = new ClaimsPrincipal(claimIdentity);
			await httpContext.SignInAsync("Cookies", claimPrincipal, new AuthenticationProperties
			{
				IsPersistent = true, 
				ExpiresUtc = DateTime.UtcNow.AddDays(2)
			}); 
		}
		public async Task SignOutAsync(HttpContext httpContext)
		{
			await httpContext.SignOutAsync("Cookies");
		}
		public SystemUser? GetCurrentUser(HttpContext httpContext)
		{
			var userIdClaim = httpContext.User.FindFirst("UserId"); 
			if(userIdClaim == null)
			{
				return null;
			}
			if (int.TryParse(userIdClaim.Value, out int userId))
			{
				return _context.SystemUsers.Find(userId);
			}
			return null;
		}
		public bool IsAdmin(SystemUser user)
		{
			return user.Role?.ToLower() == "admin"; 
		}
		public bool IsUser(SystemUser user)
		{
			return user.Role?.ToLower() == "user" || IsAdmin(user);
		}
	}
}
