using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Emuns;
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
        private List<Room> _availableRooms;

        public RoomBookingProcessorTest()
        {
            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2023, 10, 20)
            };

            _availableRooms = new List<Room>() { new Room() { Id = 1 } };
            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(r => r.GetAvailableRooms(_request.Date)).Returns(_availableRooms); //returns the list, so needs .Returns() in setup
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
            _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>())) //Save just carries out an action, no .Returns() needed in setup
                .Callback<RoomBooking>(booking =>
                {
                    savedBooking = booking;
                });

            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once); //verify that the save method was called once.

            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(_request.FullName);
            savedBooking.Email.ShouldBe(_request.Email);
            savedBooking.Date.ShouldBe(_request.Date);
            savedBooking.RoomId.ShouldBe(_availableRooms.First().Id);
        }

        [Fact]
        public void Should_Not_Save_Room_Booking_Request_If_None_Available()
        {
            _availableRooms.Clear(); //"clear" the list of rooms because it is testing that there are NO avialable rooms
            _processor.BookRoom(_request);
            //Verify the Assertion in this case
            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never); //because there are no available rooms save is never called

        }

        [Theory]
        [InlineData(BookingResultFlag.Failure, false)] //data scenario to the test beforehand. Auto infer the scenario
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Results(BookingResultFlag bookingSuccessFlag, bool isAvailable) //data driven test example.Params instead of mocks. Params: is the first value(success) when the second value is that(available)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear(); //if none available clear the list so that save is never called. Using none available test structure
            }

            var result = _processor.BookRoom(_request);
            bookingSuccessFlag.ShouldBe(result.Flag); //flag added to the result object. Emun always defaults to 1st one(success in this case)
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_RoomBookingID_In_Result(int? roomBookingID, bool isAvailable) 
        {
            if (!isAvailable)
            {
                _availableRooms.Clear(); //if none available clear the list so that save is never called.
            }
            else
            {
                _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    booking.Id = roomBookingID.Value;
                });
            }

            var result = _processor.BookRoom(_request);
            roomBookingID.ShouldBe(result.RoomBookingId);
        }


    }
}
