using Newtonsoft.Json;

namespace FlightTicketsWeb.Web.ViewModels.Weather
{
	public class WeatherData
	{
		[JsonProperty("name")]
		public string CityName { get; set; }
		[JsonProperty("main")]
		public MainWeatherData Main { get; set; }
		[JsonProperty("weather")]
		public WeatherDescription[] Weather { get; set; }
	}
}
