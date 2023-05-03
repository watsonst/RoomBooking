using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core
{
    public class RoomBookingProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;

        public RoomBookingProcessorTest()
        {
            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2023, 10, 20)
            };

            _roomBookingServiceMock = new Mock<IRoomBookingService>(); //need to inject this service into our processor
            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
        }

        [Fact]
        public void Should_Return_Room_Booking_Responce_With_Request_Values()
        {

            //Act
            RoomBookingResult result = _processor.BookRoom(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FullName, result.FullName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);

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

        [Fact]
        public void Should_Save_Room_Booking_Request()
        {
            RoomBooking savedBooking = null;
            _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>())) //How to do want the method to behave. What should this method so in a testing environment. Any object of type RoomBooking is allowed
                .Callback<RoomBooking>(booking =>
                {
                    savedBooking = booking;
                }); //savedBooking should get the value whatever is being passed in from Save and set that to booking var in test 

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once); //verify that the save method was called once.

            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(_request.FullName);
            savedBooking.Email.ShouldBe(_request.Email);
            savedBooking.Date.ShouldBe(_request.Date);

        }
    }
}
