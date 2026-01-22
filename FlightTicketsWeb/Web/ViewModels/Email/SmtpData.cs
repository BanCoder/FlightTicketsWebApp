namespace FlightTicketsWeb.Web.ViewModels.Email
{
	public class SmtpData
	{
		public SmtpConfiguration Gmail {  get; set; }
		public SmtpConfiguration MailRu { get; set; }
		public SmtpConfiguration Yandex { get; set; }
	}
}
