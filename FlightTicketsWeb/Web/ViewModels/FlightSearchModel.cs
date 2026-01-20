using System.ComponentModel.DataAnnotations;

namespace FlightTicketsWeb.Web.ViewModels
{
	public class FlightSearchModel
	{
		public string? FromCity { get; set; }
		public string? ToCity { get; set; }
		public DateTime? DepartureDate { get; set; }
		public DateTime? ReturnDate	{ get; set; }
		public int PassangersCount { get; set; }
		public string? Class {  get; set; }
	}
}
