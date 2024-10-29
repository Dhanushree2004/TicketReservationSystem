using Microsoft.AspNetCore.Mvc;
using TicketReservationAPI.Data;
using TicketReservationAPI.Models;

namespace TicketReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly TicketReservationContext _context;

        public BookingsController(TicketReservationContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Booking>> BookTickets(Booking booking)
        {
            var eventToUpdate = await _context.Events.FindAsync(booking.EventId);
            if (eventToUpdate == null || eventToUpdate.AvailableSeats < booking.NumberOfTickets)
            {
                return BadRequest("Not enough available seats.");
            }

            eventToUpdate.AvailableSeats -= booking.NumberOfTickets;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, booking);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            return booking;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            var eventToUpdate = await _context.Events.FindAsync(booking.EventId);
            eventToUpdate.AvailableSeats += booking.NumberOfTickets;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
