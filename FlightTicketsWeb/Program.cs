using FlightTicketsWeb.Repository;
using FlightTicketsWeb.Repository.Access;
using FlightTicketsWebsite.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); 
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("Project"));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<ITravelRepository, TravelRepository>();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", options =>
{
	options.Cookie.Name = "AirTravelAuth";
	options.LoginPath = "/Account/Entrance";
	options.AccessDeniedPath = "/Account/AccessDenied";
	options.ExpireTimeSpan = TimeSpan.FromDays(2);
	options.Cookie.HttpOnly = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("UserOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "user" || c.Value == "admin"))));
	options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role,"admin"));
});
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
