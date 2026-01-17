namespace FlightTicketsWeb.Models
{
	public class HotelViewModel
	{
		public int HotelId { get; set; }
		public string? HotelName { get; set; }
		public string? City { get; set; }
		public string? Address { get; set; }
		public decimal CostPerNight { get; set; }
		public int? Stars { get; set; }
		public string? Category { get; set; }
		public int RoomsAvailable { get; set; }
		public string? ImageUrl { get; set; }
	}
}
