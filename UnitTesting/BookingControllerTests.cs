using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketReservationAPI.Controllers;
using TicketReservationAPI.Data;
using TicketReservationAPI.Models;

namespace TUnitTesting
{
    [TestFixture]
    public class BookingsControllerTests
    {
        private TicketReservationContext _context;
        private BookingsController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketReservationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TicketReservationContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Events.Add(new Event
            {
                EventId = 13,
                Name = "Hiphop Aadhi Concert",
                Venue = "City Amphitheater",
                TotalSeats = 350,
                AvailableSeats = 210,
                Date = new DateTime(2024, 11, 30)
            });
            _context.SaveChanges();

            _controller = new BookingsController(_context);
        }

        [Test]
        public async Task CreateBooking_AddsNewBookingSuccessfully()
        {
            var newBooking = new Booking
            {
                BookingId = 1,
                EventId = 13,
                NumberOfTickets = 2,
                UserReference = "UserXYZ",
                BookingDate = DateTime.Now
            };

            var result = await _controller.BookTickets(newBooking);

            Assert.That(result, Is.InstanceOf<ActionResult<Booking>>());
            var actionResult = result.Result as CreatedAtActionResult;
            Assert.That(actionResult?.Value, Is.InstanceOf<Booking>());

            var createdBooking = actionResult?.Value as Booking;
            Assert.That(createdBooking?.BookingId, Is.EqualTo(1));
            Assert.That(createdBooking?.EventId, Is.EqualTo(13));
            Assert.That(createdBooking?.UserReference, Is.EqualTo("UserXYZ"));

            var bookingInDb = await _context.Bookings.FindAsync(1);
            Assert.That(bookingInDb, Is.Not.Null);
            Assert.That(bookingInDb?.UserReference, Is.EqualTo("UserXYZ"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }

    [TestFixture]
    public class EventsControllerTests
    {
        private TicketReservationContext _context;
        private EventsController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketReservationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TicketReservationContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Events.AddRange(
                new Event
                {
                    EventId = 9,
                    Name = "Anirudh Concert",
                    Date = new DateTime(2024, 12, 13),
                    Venue = "Coddissia",
                    TotalSeats = 500,
                    AvailableSeats = 490,
                },
                new Event
                {
                    EventId = 10,
                    Name = "Yuvan Shankar Raja Concert",
                    Date = new DateTime(2024, 12, 22),
                    Venue = "Auditorium",
                    TotalSeats = 400,
                    AvailableSeats = 400,
                }
            );
            _context.SaveChanges();

            _controller = new EventsController(_context);
        }

        [Test]
        public async Task GetEvents_ReturnsAllEvents()
        {
            var result = await _controller.GetEvents();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var actionResult = result.Result as OkObjectResult;
            Assert.That(actionResult?.Value, Is.InstanceOf<IEnumerable<Event>>());

            var events = actionResult?.Value as List<Event>;
            Assert.That(events?.Count, Is.EqualTo(2));
            Assert.That(events?[0].Name, Is.EqualTo("Anirudh Concert"));
            Assert.That(events?[1].Name, Is.EqualTo("Yuvan Shankar Raja Concert"));
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
