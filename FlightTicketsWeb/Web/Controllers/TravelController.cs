using FlightTicketsWeb.Core.Interfaces;
using FlightTicketsWeb.Web.ViewModels;
using FlightTicketsWeb.Web.ViewModels.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Web.Controllers
{
	public class TravelController : Controller
	{
		private readonly FlightTicketsContext _context;
		private readonly IWeatherService _service;
		public TravelController(FlightTicketsContext context, IWeatherService service)
		{
			_context = context;
			_service = service;
		}
		[HttpGet]
		public async Task<IActionResult> Istanbul()
		{
			var istanbul_flight = await _context.Flights.Where(f => f.DepartureCity == "Санкт-Петербург" && f.ArrivalCity == "Стамбул").OrderBy(f => f.Price).FirstOrDefaultAsync();
			var viewModel = new FlightViewModel
			{
				Price = (decimal)istanbul_flight.Price,
				DepartureDate = istanbul_flight.DepartureDate,
				ArrivalDate = istanbul_flight.ArrivalDate, 
				Weather = await _service.GetWeatherDataAsync("Стамбул")
			};
			return View(viewModel);
		}
		[HttpGet]
		public async Task<IActionResult> Maldives()
		{
			var male_flight = await _context.Flights.Where(f => f.DepartureCity == "Москва" && f.ArrivalCity == "Мале").OrderBy(f => f.Price).FirstOrDefaultAsync();
			var viewModel = new FlightViewModel
			{
				Price = (decimal)male_flight.Price,
				DepartureDate = male_flight.DepartureDate,
				ArrivalDate = male_flight.ArrivalDate, 
				Weather = await _service.GetWeatherDataAsync("Мале")
			};
			return View(viewModel);
		}
		[HttpGet]
		public async Task<IActionResult> St_Petersburg()
		{
			var spb_flight = await _context.Flights.Where(f => f.DepartureCity == "Москва" && f.ArrivalCity == "Санкт-Петербург").OrderBy(f => f.Price).FirstOrDefaultAsync();
			var viewModel = new FlightViewModel
			{
				Price = (decimal)spb_flight.Price,
				DepartureDate = spb_flight.DepartureDate,
				ArrivalDate = spb_flight.ArrivalDate, 
				Weather = await _service.GetWeatherDataAsync("Санкт-Петербург")
			};
			return View(viewModel);
		}
	}
}
