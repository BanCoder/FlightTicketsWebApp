using FlightTicketsWeb.Models;
using FlightTicketsWeb.Repository;
using FlightTicketsWeb.Repository.Access;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Controllers
{
	public class SearchController : Controller
	{
		private readonly ITravelRepository _repository;
		public SearchController(ITravelRepository repository)
		{
			_repository = repository;
		}
		[HttpGet]
		public async Task<IActionResult> FlightSearch(IndexViewModel model)
		{
			var searchModel = model.FlightSearch; 
			if (!ModelState.IsValid)
			{
				return View("Index", searchModel); 
			}
			var flights = await _repository.SearchFlightsAsync(searchModel);

			var viewModel = flights.Select(f => new FlightViewModel
			{
				FlightId = f.FlightId,
				FlightNumber = f.FlightNumber,
				AirlineName = f.AirlineNameNavigation?.CompName ?? f.AirlineName,
				DepartureCity = f.DepartureCityNavigation?.CityName ?? f.DepartureCity,
				ArrivalCity = f.ArrivalCityNavigation?.CityName ?? f.ArrivalCity,
				DepartureDate = f.DepartureDate,
				ArrivalDate = f.ArrivalDate,
				Class = f.Class,
				Price = (decimal)f.Price,
				SeatsAvailable = (int)f.SeatsAvailable
			}).ToList();

			ViewBag.SearchModel = searchModel;
			return View("SearchFlightResult", viewModel);
		}
		[HttpGet]
		public async Task<IActionResult> HotelSearch(IndexViewModel model)
		{
			var searchModel = model.HotelSearch; 
			var hotels = await _repository.SearchHotelAsync(searchModel);
			var viewModel = hotels.Select(h => new HotelViewModel
			{
				HotelId = h.HotelId,
				HotelName = h.HotelName,
				City = h.CityNameNavigation?.CityName ?? h.CityName,
				Address = h.HotelAddress,
				CostPerNight = h.CostPerNight ?? 0,
				Stars = h.Stars,
				Category = h.Category,
				RoomsAvailable = h.RoomsAvailable ?? 0,
				ImageUrl = h.ImageUrl
			}).ToList();

			ViewBag.SearchModel = searchModel;
			return View("SearchHotelResult", viewModel);
		}
	}
}
