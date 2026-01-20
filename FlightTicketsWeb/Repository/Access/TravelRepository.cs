using FlightTicketsWeb.Models;
using FlightTicketsWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Repository.Access
{
	public class TravelRepository: ITravelRepository
	{
		private readonly FlightTicketsContext _context;
		public TravelRepository(FlightTicketsContext context)
		{
			_context = context;
		}
		public async Task<List<Flight>> SearchFlightsAsync(FlightSearchModel searchModel)
		{
			var query = _context.Flights
				.Include(f => f.AirlineNameNavigation)
				.Include(f => f.DepartureCityNavigation)
				.Include(f => f.ArrivalCityNavigation)
				.AsQueryable();

			if (!string.IsNullOrEmpty(searchModel.FromCity))
			{
				query = query.Where(f =>
					f.DepartureCity.ToLower() == searchModel.FromCity.ToLower() ||
					f.DepartureCityNavigation.CityName.ToLower() == searchModel.FromCity.ToLower());
			}

			if (!string.IsNullOrEmpty(searchModel.ToCity))
			{
				query = query.Where(f =>
					f.ArrivalCity.ToLower() == searchModel.ToCity.ToLower() ||
					f.ArrivalCityNavigation.CityName.ToLower() == searchModel.ToCity.ToLower());
			}
			if (searchModel.DepartureDate.HasValue)
			{
				var date = searchModel.DepartureDate.Value.Date;
				query = query.Where(f => f.DepartureDate.Date == date);
			}
			if (!string.IsNullOrEmpty(searchModel.Class))
			{
				query = query.Where(f => f.Class == searchModel.Class);
			}

			query = query.OrderBy(f => f.DepartureDate).ThenBy(f => f.Price);

			return await query.ToListAsync();
		}
		public async Task<Flight> GetFlightByIdAsync(int flightId)
		{
			return await _context.Flights.Include(f => f.ArrivalCityNavigation).FirstOrDefaultAsync(f => f.FlightId == flightId);
		}
		public async Task UpdateFlightSeatsAsync(int flightId, int seatsChange)
		{
			var flight = await _context.Flights.FindAsync(flightId);
			if (flight != null)
			{
				flight.SeatsAvailable += seatsChange;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<List<Hotel>> SearchHotelAsync(HotelSearchModel searchModel)
		{
			var query = _context.Hotels
			.Include(h => h.CityNameNavigation)
			.AsQueryable();

			if (!string.IsNullOrEmpty(searchModel.Direction))
			{
				query = query.Where(h =>
					h.CityName.Contains(searchModel.Direction) ||
					h.CityNameNavigation.CityName.Contains(searchModel.Direction) ||
					h.HotelName.Contains(searchModel.Direction));
			}
			if (!string.IsNullOrEmpty(searchModel.Category))
			{
				if (int.TryParse(searchModel.Category, out int stars))
				{
					query = query.Where(h => h.Stars == stars);
				}
				else if (searchModel.Category.ToLower() == "апартаменты")
				{
					query = query.Where(h => h.Category == null || h.Category.Contains("Апартаменты"));
				}
				else
				{
					query = query.Where(h => h.Category != null && h.Category.Contains(searchModel.Category));
				}
			}
			return await query.OrderBy(h => h.CostPerNight).ToListAsync();
		}
		public async Task<Hotel> GetHotelByIdAsync(int hotelId)
		{
			return await _context.Hotels.FindAsync(hotelId);
		}
		public async Task UpdateHotelRoomsAsync(int hotelId, int roomsChange)
		{
			var hotel = await _context.Hotels.FindAsync(hotelId);
			if (hotel != null && hotel.RoomsAvailable.HasValue)
			{
				hotel.RoomsAvailable += roomsChange;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<Passenger> GetOrCreatePassengerAsync(Passenger passengerData)
		{
			var existingPassenger = await _context.Passengers.FirstOrDefaultAsync(p => p.PassportNum == passengerData.PassportNum);

			if (existingPassenger != null)
				return existingPassenger;

			passengerData.CreatedAt = DateTime.Now;
			_context.Passengers.Add(passengerData);
			await _context.SaveChangesAsync();
			return passengerData;
		}
		public async Task CreateBookingAsync(Booking booking)
		{
			_context.Bookings.Add(booking);
			await _context.SaveChangesAsync();
		}
		public async Task<string> GenerateBookingCodeAsync()
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
		}

	}
}
