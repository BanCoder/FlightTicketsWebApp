using System;
using System.Collections.Generic;

namespace FlightTicketsWeb.Core.Entities;

public partial class City
{
    public string CityName { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? Region { get; set; }

    public string? Climate { get; set; }

    public string? AirportCode { get; set; }

    public virtual ICollection<Flight> FlightArrivalCityNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightDepartureCityNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
