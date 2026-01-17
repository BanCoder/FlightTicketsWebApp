using FlightTicketsWeb.Models;
using FlightTicketsWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Controllers
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
