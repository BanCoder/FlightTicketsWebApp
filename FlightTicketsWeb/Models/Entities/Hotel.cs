using System;
using System.Collections.Generic;

namespace FlightTicketsWeb.Models.Entities;

public partial class Hotel
{
    public int HotelId { get; set; }

    public string HotelName { get; set; } = null!;

    public string HotelAddress { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public decimal? CostPerNight { get; set; }

    public int? Stars { get; set; }

    public string? Category { get; set; }

    public int? RoomsAvailable { get; set; }
    public string? ImageUrl { get; set; }
	public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual City CityNameNavigation { get; set; } = null!;
}
