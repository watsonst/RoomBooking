using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core
{
    public class RoomBookingProcessorTest
    {
        private RoomBookingRequestProcessor _processor;

        public RoomBookingProcessorTest()
        {
            //Arrange one time in constructor and use arranged object everywhere else
            _processor = new RoomBookingRequestProcessor();
        }

        [Fact]
        public void Should_Return_Room_Booking_Responce_With_Request_Values()
        {
            //Arrange
            var request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2023, 10, 20)
            };

            //Act
            RoomBookingResult result = _processor.BookRoom(request);

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

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            //No Arragne because the request should be null

            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));

            //Assert
            exception.ParamName.ShouldBe("bookingRequest");

            //Assert.Throws<ArgumentNullException>(() => processor.BookRoom(null));
            //Should.Throw<ArgumentNullException>(() => processor.BookRoom(null));
        }
    }
}
