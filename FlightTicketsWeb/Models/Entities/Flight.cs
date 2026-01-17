using System;
using System.Collections.Generic;

namespace FlightTicketsWeb.Models.Entities;

public partial class Flight
{
    public int FlightId { get; set; }

    public string FlightNumber { get; set; } = null!;

    public string AirlineName { get; set; } = null!;

    public string DepartureCity { get; set; } = null!;

    public string ArrivalCity { get; set; } = null!;

    public DateTime DepartureDate { get; set; }

    public DateTime ArrivalDate { get; set; }

    public string? Class { get; set; }

    public decimal? Price { get; set; }

    public int? SeatsAvailable { get; set; }

    public virtual Airline AirlineNameNavigation { get; set; } = null!;

    public virtual City ArrivalCityNavigation { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual City DepartureCityNavigation { get; set; } = null!;
}
