using FlightTicketsWeb.Core.Entities;
using FlightTicketsWeb.Web.ViewModels;

namespace FlightTicketsWeb.Core.Interfaces
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
