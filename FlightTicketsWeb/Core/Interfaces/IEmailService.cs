using FlightTicketsWeb.Web.ViewModels.Email;

namespace FlightTicketsWeb.Core.Interfaces
{
	public interface IEmailService
	{
		void SendSuccessEmail(string userLogin, string firstName);
		string GetEmailDomain(string email);
	}
}
