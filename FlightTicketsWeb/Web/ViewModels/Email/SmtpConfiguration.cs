namespace FlightTicketsWeb.Web.ViewModels.Email
{
	public class SmtpConfiguration
	{
		public string Server { get; set; }
		public int Port { get; set; }
		public string AdminLogin { get; set; }
		public string Password { get; set; }
	}
}
