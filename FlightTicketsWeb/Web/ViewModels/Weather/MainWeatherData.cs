using Newtonsoft.Json;
namespace FlightTicketsWeb.Web.ViewModels.Weather
{
	public class MainWeatherData
	{
		[JsonProperty("temp")]
		public float Temperature { get; set; }
		[JsonProperty("humidity")]
		public int Humidity { get; set; }
	}
}
