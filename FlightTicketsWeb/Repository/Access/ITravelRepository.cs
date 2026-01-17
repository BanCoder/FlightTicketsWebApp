using FlightTicketsWeb.Models;
using FlightTicketsWeb.Models.Entities;

namespace FlightTicketsWeb.Repository.Access
{
	public interface ITravelRepository
	{
		Task<List<Flight>> SearchFlightsAsync(FlightSearchModel searchModel);
		Task<Flight> GetFlightByIdAsync(int flightId);
		Task UpdateFlightSeatsAsync(int flightId, int seatsChange);

		Task<List<Hotel>> SearchHotelAsync(HotelSearchModel searchModel);
		Task<Hotel> GetHotelByIdAsync(int hotelId);
		Task UpdateHotelRoomsAsync(int hotelId, int roomsChange);

		Task<Passenger> GetOrCreatePassengerAsync(Passenger passengerData);
		Task CreateBookingAsync(Booking booking);
		Task<string> GenerateBookingCodeAsync();
	}
}
