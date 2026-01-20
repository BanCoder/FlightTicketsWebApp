namespace FlightTicketsWeb.Web.ViewModels
{
	public class BookingModel
	{
		public string PassportNum { get; set; }
		public string Surname { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateOnly BirthDate { get; set; }
		public string Sex { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		public int FlightId { get; set; }
		public int? HotelId { get; set; }
	}
}
