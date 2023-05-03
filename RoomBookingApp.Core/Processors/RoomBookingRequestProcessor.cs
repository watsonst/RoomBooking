using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            this._roomBookingService = roomBookingService;
        }

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            if(bookingRequest == null)
            {   
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            _roomBookingService.Save(CreateRoomBookingObject<RoomBooking>(bookingRequest)); //this replaces previous explicit declarations

            return CreateRoomBookingObject<RoomBookingResult>(bookingRequest);
  
        }

        private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking 
            : RoomBookingBase, new() //anything inheriting from roomBookBase can be used an a generic here
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Email = bookingRequest.Email,
                Date = bookingRequest.Date,
            };
        }
        
    }
}