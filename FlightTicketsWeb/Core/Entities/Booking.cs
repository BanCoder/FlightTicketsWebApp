using System;
using System.Collections.Generic;

namespace FlightTicketsWeb.Core.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public DateTime? BookingDate { get; set; }

    public string? BookingCode { get; set; }

    public int? FlightId { get; set; }

    public string PassengerPassport { get; set; } = null!;

    public int? HotelId { get; set; }

    public virtual Flight? Flight { get; set; } = null!;

    public virtual Hotel? Hotel { get; set; }

    public virtual Passenger PassengerPassportNavigation { get; set; } = null!;
}
