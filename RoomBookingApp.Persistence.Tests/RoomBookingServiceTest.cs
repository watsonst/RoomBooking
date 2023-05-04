using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest //integration tests. How does the app interact with the orm/library(EF) that communicates to database
    {
        [Fact]
        public void Should_Return_Available_Rooms()
        {
            //Arrange sudo/staged database call
            var date = new DateTime(2023, 03, 09);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { RoomId = 1, FullName = "Test Name1", Email = "test1@email.com", Date = date });
            context.Add(new RoomBooking { RoomId = 2, FullName = "Test Name2", Email = "test2@email.com", Date = date.AddDays(-1) });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            //Act

            var availableRooms = roomBookingService.GetAvailableRooms(date);

            //Assert
            Assert.Equal(2, availableRooms.Count()); //on this date should have 2 rooms available
            Assert.Contains(availableRooms, q => q.Id == 2);
            Assert.Contains(availableRooms, q => q.Id == 3);
            Assert.DoesNotContain(availableRooms, q => q.Id == 1);

            //could assert agianst the name
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
              .UseInMemoryDatabase("ShouldSaveTest")
              .Options;

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2023, 03, 09), FullName = "Test Name1", Email = "test1@email.com" };

            //Act
            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);
            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings); //only one in the room bookings list and returns that one

            //Assert
            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
          
        }

    }
}
