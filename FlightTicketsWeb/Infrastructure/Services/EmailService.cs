using FlightTicketsWeb.Core.Interfaces; 

using FlightTicketsWeb.Web.ViewModels.Email;
using FlightTicketsWebsite.Infrastructure;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Mail;
namespace FlightTicketsWeb.Infrastructure.Services
{
	public class EmailService: IEmailService
	{
		private IConfiguration _configuration;
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public void SendSuccessEmail(string userLogin, string firstName)
		{
			string emailDomain = GetEmailDomain(userLogin);
			AppConfiguration? appConfiguration = _configuration.GetSection("Project").Get<AppConfiguration>();
			SmtpConfiguration smtpConfiguration = new SmtpConfiguration();
			switch (emailDomain)
			{
				case "gmail.com":
					smtpConfiguration = appConfiguration.SmtpSettings.Gmail;
					break; 
				case "mail.ru":
					smtpConfiguration = appConfiguration.SmtpSettings.MailRu;
					break;
				case "yandex.ru":
					smtpConfiguration = appConfiguration.SmtpSettings.Yandex;
					break;
				default:
					throw new Exception("Неподдерживаемый почтовый домен"); 
			}
			using (SmtpClient smtpClient = new SmtpClient(smtpConfiguration.Server, smtpConfiguration.Port))
			{
				smtpClient.Credentials = new NetworkCredential(smtpConfiguration.AdminLogin, smtpConfiguration.Password);
				smtpClient.EnableSsl = true; 
				using(MailMessage mailMessage = new MailMessage())
				{
					mailMessage.From = new MailAddress(smtpConfiguration.AdminLogin);
					mailMessage.To.Add(userLogin);
					mailMessage.Subject = "Подтверждение бронирования";
					mailMessage.Body = $"Здравствуйте, {firstName}!\r\nВаша бронь прошла успешно! Спасибо, что выбрали наш серивис.\r\n\n\nС уважением, компания AirTravel."; 
					smtpClient.Send(mailMessage);
				}
			}
		}
		public string GetEmailDomain(string email)
		{
			MailAddress mailAddress = new MailAddress(email);
			return mailAddress.Host; 
		}
	}
}
