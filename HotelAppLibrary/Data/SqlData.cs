using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                  new { startDate, endDate },
                                                  connectionStringName,
                                                  true);
        }

        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            // Create Guest
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_InsertGuest",
                                                                 new { firstName, lastName },
                                                                 connectionStringName,
                                                                 true).First();
            // Get Room Type
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id = @Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName,
                                                                          false).First();
            // Calculate how long they want to stay
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            // Get available rooms for date range selected
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                              new { startDate, endDate, roomTypeId },
                                                                              connectionStringName,
                                                                              true);

            // Create Booking
            _db.SaveData("dbo.spBookings_Insert",
                         new
                         {
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             totalCost = timeStaying.Days * roomType.Price
                         },
                         connectionStringName,
                         true);
        }

        public List<BookingFullModel> FindBookings(string lastName)
        {
            return _db.LoadData<BookingFullModel, dynamic>("dbo.spBookings_FindBookings",
                                                                        new { lastName, startDate = DateTime.Now.Date },
                                                                        connectionStringName,
                                                                        true);
        }

        public void CheckInGuest(int bookingId)
        {

            _db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName, true);
        }

    }
}
