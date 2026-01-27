using FlightTicketsWeb.Core.Entities;
using FlightTicketsWeb.Core.Interfaces;
using FlightTicketsWeb.Core;
using FlightTicketsWeb.Shared;
using FlightTicketsWeb.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightTicketsWeb.Web.Controllers
{
	[Authorize(Policy ="UserOnly")]
	public class BookingController : Controller
	{
		private readonly ITravelRepository _repository;
		private readonly IAuthService _authService;
		private readonly IEmailService _emailService;
		private readonly ILogger<BookingController> _logger;
		public BookingController(ITravelRepository repository, IAuthService authService, IEmailService emailService, ILogger<BookingController> logger)
		{
			_repository = repository;
			_authService = authService;
			_emailService = emailService;
			_logger = logger;
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
				ViewBag.DepartureCity = flight.DepartureCity;
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
				await PopulateViewBagForBooking(model.FlightId);
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
				try
				{
					_emailService.SendSuccessEmail(model.Email, model.FirstName, booking.BookingCode);
					_logger.LogInformation("Успешная отправка письма с подтверждением брони рейсов пользователю {Email}", model.Email); 
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = "Не удалось отправить подтверждение на email";
					_logger.LogError("Ошибка отправки письма подтверждения брони рейсов пользователя {Email}:", model.Email); 
				}
				ViewBag.FlightId = model.FlightId ;
				var flight = await _repository.GetFlightByIdAsync(model.FlightId);
				if (flight != null)
				{
					var hotels = await _repository.SearchHotelAsync(new HotelSearchModel
					{
						Direction = flight.ArrivalCity
					});
					ViewBag.Hotels = hotels;
					ViewBag.ArrivalCity = flight.ArrivalCity;
					ViewBag.DepartureCity = flight.DepartureCity;
				}
				await PopulateViewBagForBooking(model.FlightId);
				ViewBag.ShowAlert = true;
				ViewBag.SuccessMessage = "Бронирование успешно создано! Мы отправили Вам сообщение на почту.";
				ViewBag.BookingCode = booking.BookingCode;
				_logger.LogInformation("Успешное бронирование рейса пользователя {Email}. Код брони: {BookingCode}", model.Email, booking.BookingCode); 
				return View(model);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Произошла ошибка: {ex.Message}");
				_logger.LogError($"Ошибка бронирования рейсов: {ex.Message}"); 
				await PopulateViewBagForBooking(model.FlightId);
				return View("TicketsAndHotelBooking", model);
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
				try
				{
					_emailService.SendSuccessEmail(model.Email, model.FirstName, booking.BookingCode);
					_logger.LogInformation("Успешная отправка письма с подтверждением брони отелей пользователю {Email}", model.Email);
				}
				catch (Exception ex)
				{
					ViewBag.EmailError = "Не удалось отправить подтверждение на email";
					_logger.LogError("Ошибка отправки письма подтверждения брони рейсов пользователя {Email}", model.Email);
				}
				ViewBag.BookingCode = booking.BookingCode;
				ViewBag.SuccessMessage = "Отель забронирован! Мы отправили Вам сообщение на почту.";
				ViewBag.ShowAlert = true;
				ViewBag.SelectedHotel = hotel;
				_logger.LogInformation("Успешное бронирование отеля пользователя {Email}. Код брони: {BookingCode}", model.Email, booking.BookingCode);
				return View("HotelOnlyBooking", model);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Ошибка: {ex.Message}");
				_logger.LogError($"Ошибка бронирования отелей: {ex.Message}");
				ViewBag.SelectedHotel = hotel;
				ViewBag.HotelId = model.HotelId;
				return View("HotelOnlyBooking", model);
			}
		}
		public async Task PopulateViewBagForBooking(int flightId)
		{
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
				ViewBag.DepartureCity = flight.DepartureCity;
			}
		}
	}
}
