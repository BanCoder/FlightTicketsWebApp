using FlightTicketsWeb.Core.Entities;
using FlightTicketsWeb.Web.ViewModels.Weather;

namespace FlightTicketsWeb.Web.ViewModels
{
	public class FlightViewModel
	{
		public int FlightId { get; set; }
		public string? FlightNumber { get; set; }
		public string? AirlineName { get; set; }
		public string? DepartureCity { get; set; }
		public string? ArrivalCity { get; set; }
		public DateTime DepartureDate { get; set; }
		public DateTime ArrivalDate { get; set; }
		public string? Class { get; set; }
		public decimal Price { get; set; }
		public int SeatsAvailable { get; set; }
		public WeatherData? Weather { get; set; }
		public Airline? AirlineUrl { get; set; }


		public string FormattedDepartureTime => DepartureDate.ToString("HH:mm");
		public string FormattedArrivalTime => ArrivalDate.ToString("HH:mm");
		public string Duration => $"{(int)(ArrivalDate - DepartureDate).TotalHours}ч {(ArrivalDate - DepartureDate).Minutes}м";
		public string FormattedPrice => Price.ToString("N0") + " ₽";
	}
}
