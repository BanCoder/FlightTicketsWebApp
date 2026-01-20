using FlightTicketsWeb.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View(new IndexViewModel());
		}
		public IActionResult AboutUs()
		{
			return View(); 
		}
	}
}
