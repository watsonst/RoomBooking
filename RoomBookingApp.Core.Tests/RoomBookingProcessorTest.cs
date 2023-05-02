using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Core
{
    public class RoomBookingProcessorTest
    {
        [Fact]
        public void Should_Return_Room_Booking_Responce_With_Request_Values()
        {
            //Arrange
            var request = new RoomBookingRequest //request class/sample data
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2023, 10, 20)
            };

            var processor = new RoomBookingRequestProcessor(); //object of what we need to test

            //Act
            RoomBookingResult result = processor.BookRoom(request); //Calling method and getting result 

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.FullName, result.FullName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);

            //Shouldly library
            //result.ShouldNotBeNull(); 
            //result.FullName.ShouldBe(request.FullName);
            //result.Email.ShouldBe(request.Email);
            //result.Date.ShouldBe(request.Date);


        }
    }
}
