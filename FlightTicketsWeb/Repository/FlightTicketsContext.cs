using System;
using System.Collections.Generic;
using FlightTicketsWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Repository;

public partial class FlightTicketsContext : DbContext
{
	public FlightTicketsContext()
	{
	}

	public FlightTicketsContext(DbContextOptions<FlightTicketsContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Airline> Airlines { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{

		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airline>(entity =>
        {
            entity.HasKey(e => e.CompName).HasName("PK__Airline__93C4206FAEBCC6F9");

            entity.ToTable("Airline");

            entity.Property(e => e.CompName)
                .HasMaxLength(50)
                .HasColumnName("comp_name");
            entity.Property(e => e.AirlineAddress)
                .HasMaxLength(100)
                .HasColumnName("airline_address");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contact_number");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(200)
                .HasColumnName("logo_url");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__5DE3A5B1FCF55A08");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.BookingCode, "UQ__Booking__FF29040FB09CBED9").IsUnique();

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.BookingCode)
                .HasMaxLength(10)
                .HasColumnName("booking_code");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("booking_date");
            entity.Property(e => e.FlightId).HasColumnName("flight_id");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.PassengerPassport)
                .HasMaxLength(20)
                .HasColumnName("passenger_passport");

            entity.HasOne(d => d.Flight).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__flight___3AD6B8E2");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__Booking__hotel_i__3CBF0154");

            entity.HasOne(d => d.PassengerPassportNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PassengerPassport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__passeng__3BCADD1B");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityName).HasName("PK__City__1AA4F7B4502F63C5");

            entity.ToTable("City");

            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .HasColumnName("city_name");
            entity.Property(e => e.AirportCode)
                .HasMaxLength(10)
                .HasColumnName("airport_code");
            entity.Property(e => e.Climate)
                .HasMaxLength(30)
                .HasColumnName("climate");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flight__E37057658FB2D292");

            entity.ToTable("Flight");

            entity.Property(e => e.FlightId).HasColumnName("flight_id");
            entity.Property(e => e.AirlineName)
                .HasMaxLength(50)
                .HasColumnName("airline_name");
            entity.Property(e => e.ArrivalCity)
                .HasMaxLength(50)
                .HasColumnName("arrival_city");
            entity.Property(e => e.ArrivalDate)
                .HasColumnType("datetime")
                .HasColumnName("arrival_date");
            entity.Property(e => e.Class)
                .HasMaxLength(20)
                .HasColumnName("class");
            entity.Property(e => e.DepartureCity)
                .HasMaxLength(50)
                .HasColumnName("departure_city");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("datetime")
                .HasColumnName("departure_date");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(20)
                .HasColumnName("flight_number");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SeatsAvailable)
                .HasDefaultValue(100)
                .HasColumnName("seats_available");

            entity.HasOne(d => d.AirlineNameNavigation).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirlineName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Flight__airline___2B947552");

            entity.HasOne(d => d.ArrivalCityNavigation).WithMany(p => p.FlightArrivalCityNavigations)
                .HasForeignKey(d => d.ArrivalCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Flight__arrival___2AA05119");

            entity.HasOne(d => d.DepartureCityNavigation).WithMany(p => p.FlightDepartureCityNavigations)
                .HasForeignKey(d => d.DepartureCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Flight__departur__29AC2CE0");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.HotelId).HasName("PK__Hotel__45FE7E26A547A5A3");

            entity.ToTable("Hotel");

            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.Category)
                .HasMaxLength(30)
                .HasColumnName("category");
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .HasColumnName("city_name");
            entity.Property(e => e.CostPerNight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost_per_night");
            entity.Property(e => e.HotelAddress)
                .HasMaxLength(200)
                .HasColumnName("hotel_address");
            entity.Property(e => e.HotelName)
                .HasMaxLength(100)
                .HasColumnName("hotel_name");
            entity.Property(e => e.RoomsAvailable)
                .HasDefaultValue(50)
                .HasColumnName("rooms_available");
			entity.Property(e => e.ImageUrl)
		   .HasMaxLength(500)
		   .HasColumnName("image_url");
			entity.Property(e => e.Stars).HasColumnName("stars");

            entity.HasOne(d => d.CityNameNavigation).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CityName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Hotel__city_name__314D4EA8");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.PassportNum).HasName("PK__Passenge__28C916B62F127E48");

            entity.ToTable("Passenger");

            entity.Property(e => e.PassportNum)
                .HasMaxLength(20)
                .HasColumnName("passport_num");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .HasColumnName("sex");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Passenger__user___361203C5");
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemUs__3213E83F7BDBACC7");

            entity.ToTable("SystemUser");

            entity.HasIndex(e => e.Email, "UQ__SystemUs__AB6E6164F6EDC25A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("user")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
