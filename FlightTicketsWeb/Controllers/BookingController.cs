using FlightTicketsWeb.Models;
using FlightTicketsWeb.Models.Entities;
using FlightTicketsWeb.Repository;
using FlightTicketsWeb.Repository.Access;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightTicketsWeb.Controllers
{
	[Authorize(Policy ="UserOnly")]
	public class BookingController : Controller
	{
		private readonly ITravelRepository _repository;
		private readonly IAuthService _authService; 
		public BookingController(ITravelRepository repository, IAuthService authService)
		{
			_repository = repository;
			_authService = authService;
		}
		[HttpGet]
		public async Task<IActionResult> TicketsAndHotelBooking(int flightId)
		{
			var currentUser = _authService.GetCurrentUser(HttpContext);
			if (currentUser == null)
			{
				return RedirectToAction("Entrance", "Account");
			}
			ViewBag.FlightId = flightId;
			var flight = await _repository.GetFlightByIdAsync(flightId);
			if (flight != null)
			{
				var hotels = await _repository.SearchHotelAsync(new HotelSearchModel
				{
					Direction = flight.ArrivalCity
				});
				ViewBag.Hotels = hotels;
				ViewBag.ArrivalCity = flight.ArrivalCity;
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> TicketsAndHotelBooking(BookingModel model)
		{
			var currentUser = _authService.GetCurrentUser(HttpContext);
			if (currentUser == null)
			{
				return RedirectToAction("Entrance", "Account");
			}
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			try
			{
				var passenger = new Passenger
				{
					PassportNum = model.PassportNum,
					Surname = model.Surname,
					FirstName = model.FirstName,
					LastName = model.LastName,
					BirthDate = model.BirthDate,
					Sex = model.Sex,
					Phone = model.Phone,
					Email = model.Email,
					UserId = currentUser.Id

				};
				await _repository.GetOrCreatePassengerAsync(passenger);
				var booking = new Booking
				{
					FlightId = model.FlightId,
					PassengerPassport = model.PassportNum,
					HotelId = model.HotelId,
					BookingDate = DateTime.Now,
					BookingCode = await _repository.GenerateBookingCodeAsync()
				}; 
				await _repository.CreateBookingAsync(booking);
				if (model.FlightId > 0)
				{
					await _repository.UpdateFlightSeatsAsync(model.FlightId, -1);
				}
				if (model.HotelId.HasValue)
				{
					await _repository.UpdateHotelRoomsAsync(model.HotelId.Value, -1);
				}
				ViewBag.ShowAlert = true;
				ViewBag.SuccessMessage = "Бронирование успешно создано!";
				ViewBag.BookingCode = booking.BookingCode;
				
				return View(model);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Произошла ошибка: {ex.Message}");
				return View(model);
			}
		}
		[HttpGet]
		public async Task<IActionResult> BookOnlyHotel(int hotelId)
		{
			var hotel = await _repository.GetHotelByIdAsync(hotelId);
			if (hotel == null)
			{
				return RedirectToAction("Index", "Hotels");
			}
			var model = new BookingModel
			{
				HotelId = hotelId
			};
			ViewBag.SelectedHotel = hotel;
			ViewBag.HotelId = hotelId;
			ViewBag.OnlyHotel = true;
			return View("HotelOnlyBooking", model);
		}
		[HttpPost]
		public async Task<IActionResult> BookOnlyHotelPost(BookingModel model)
		{
			Hotel? hotel = null;
			try
			{
				hotel = await _repository.GetHotelByIdAsync(model.HotelId.Value); 

				if (!ModelState.IsValid)
				{
					ViewBag.SelectedHotel = hotel;
					ViewBag.HotelId = model.HotelId;
					return View("HotelOnlyBooking", model);
				}
				var passenger = new Passenger
				{
					PassportNum = model.PassportNum,
					Surname = model.Surname,
					FirstName = model.FirstName,
					LastName = model.LastName,
					BirthDate = model.BirthDate,
					Sex = model.Sex,
					Phone = model.Phone,
					Email = model.Email,
				};
				await _repository.GetOrCreatePassengerAsync(passenger);
				var booking = new Booking
				{
					FlightId = null,
					PassengerPassport = model.PassportNum,
					HotelId = model.HotelId,
					BookingDate = DateTime.Now,
					BookingCode = await _repository.GenerateBookingCodeAsync()
				};
				await _repository.CreateBookingAsync(booking);
				if (hotel != null)
				{
					await _repository.UpdateHotelRoomsAsync(model.HotelId.Value, -1);
				}

				ViewBag.BookingCode = booking.BookingCode;
				ViewBag.SuccessMessage = "Отель забронирован!";
				ViewBag.ShowAlert = true;
				ViewBag.SelectedHotel = hotel;
				return View("HotelOnlyBooking", model);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Ошибка: {ex.Message}");
				ViewBag.SelectedHotel = hotel;
				ViewBag.HotelId = model.HotelId;
				return View("HotelOnlyBooking", model);
			}
		}
	}
}
