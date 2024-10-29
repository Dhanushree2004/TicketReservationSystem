namespace TicketReservationAPI.Models
{
    public class Booking
    {
        internal int AvailableSeats;

        public int BookingId { get; set; }
        public int EventId { get; set; }
        public string UserReference { get; set; }
        public int NumberOfTickets { get; set; }
        public DateTime BookingDate { get; set; }
    }

}
