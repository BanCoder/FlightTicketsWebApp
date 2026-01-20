using FlightTicketsWeb.Core.Interfaces;
using FlightTicketsWeb.Web.ViewModels.Weather;
using FlightTicketsWebsite.Infrastructure;
using System.Runtime.InteropServices;

namespace FlightTicketsWeb.Infrastructure.Services
{
	public class WeatherService: IWeatherService
	{
		private IConfiguration _configuration; 
		public WeatherService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<WeatherData?> GetWeatherDataAsync(string cityName)
		{
			AppConfiguration? configuration = _configuration.GetSection("Project").Get<AppConfiguration>();
			string apiKey = configuration.APISettings.APIKey; 
			string baseUrl = configuration.APISettings.BaseUrl;

			string requestUrl = $"{baseUrl}?q={cityName}&appid={apiKey}&units=metric&lang=ru"; 
			HttpClient client = new HttpClient();
			try
			{
				HttpResponseMessage response = await client.GetAsync(requestUrl);
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync(); 
				WeatherData weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(responseBody);
				return weatherData; 
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка получения погоды для {cityName}: {ex.Message}");
				return null;
			}
		}
	}
}
