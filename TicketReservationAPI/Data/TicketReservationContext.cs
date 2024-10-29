using Microsoft.EntityFrameworkCore;
using TicketReservationAPI.Models;

namespace TicketReservationAPI.Data
{
    public class TicketReservationContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public TicketReservationContext(DbContextOptions<TicketReservationContext> options) : base(options)
        {
        }
    }

}
