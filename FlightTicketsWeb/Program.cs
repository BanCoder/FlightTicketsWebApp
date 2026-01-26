using FlightTicketsWeb.Core.Interfaces;
using FlightTicketsWeb.Infrastructure.Services;
using FlightTicketsWeb.Shared.Helpers;
using FlightTicketsWeb.Web.ViewModels.Persistence;
using FlightTicketsWebsite.Infrastructure;
using FlightTicketsWeb.Web.Middleware; 
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorOptions(options => {
	options.ViewLocationFormats.Add("/Web/Views/{1}/{0}.cshtml");
	options.ViewLocationFormats.Add("/Web/Views/Shared/{0}.cshtml");
});
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
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IEmailService, EmailService>(); 
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
