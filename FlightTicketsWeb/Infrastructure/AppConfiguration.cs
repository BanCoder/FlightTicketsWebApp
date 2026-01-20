namespace FlightTicketsWebsite.Infrastructure
{
	public class AppConfiguration
	{
		public Company Company { get; set; } = new Company();
		public APISettings APISettings { get; set; } = new APISettings(); 
	}
	public class Company
	{
		public string? CompanyName { get; set; }
		public string? CompanyPhone { get; set; }
		public string? CompanyPhoneShort { get; set; }
		public string? CompanyEmail { get; set; }
	}
	public class APISettings
	{
		public string? APIKey { get; set; }
		public string? BaseUrl { get; set; }
	}
}
