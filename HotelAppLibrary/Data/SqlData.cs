using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqlData
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

        public void BookGuest(string firstName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomType)
        {
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_CreateGuest",
                                                                 new { firstName, lastName },
                                                                 connectionStringName,
                                                                 true).First();

        }

        //public void CreateGuest(GuestModel guest)
        //{
        //  _db.SaveData("dbo.spGuests_CreateGuest",
        //                 new { guest.FirstName, guest.LastName },
        //                 connectionStringName,
        //                 true);
        //}

        //public void CreateBooking()
        //{
            
        //}
    }
}
