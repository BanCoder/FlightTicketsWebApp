using System;
using System.Collections.Generic;

namespace FlightTicketsWeb.Models.Entities;

public partial class Airline
{
    public string CompName { get; set; } = null!;

    public string? AirlineAddress { get; set; }

    public string? ContactNumber { get; set; }

    public string? LogoUrl { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
