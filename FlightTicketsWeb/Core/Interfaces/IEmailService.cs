using FlightTicketsWeb.Web.ViewModels.Email;

namespace FlightTicketsWeb.Core.Interfaces
{
	public interface IEmailService
	{
		void SendSuccessEmail(string userLogin, string firstName, string bookingCode);
		string GetEmailDomain(string email);
	}
}
