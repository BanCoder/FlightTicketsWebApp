using FlightTicketsWeb.Web.ViewModels.Weather;

namespace FlightTicketsWeb.Core.Interfaces
{
	public interface IWeatherService
	{
		Task<WeatherData?> GetWeatherDataAsync(string cityName); 
	}
}
