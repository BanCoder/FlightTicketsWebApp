using System;
using System.Collections.Generic;

namespace FlightTicketsWeb.Core.Entities;

public partial class Passenger
{
    public string PassportNum { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string? Sex { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual SystemUser? User { get; set; }
}
