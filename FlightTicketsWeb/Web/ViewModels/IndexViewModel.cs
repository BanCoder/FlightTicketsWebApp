namespace FlightTicketsWeb.Web.ViewModels
{
	public class IndexViewModel
	{
		public FlightSearchModel FlightSearch { get; set; } = new();
		public HotelSearchModel HotelSearch { get; set; } = new(); 
	}
}
