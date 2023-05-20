using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private const string connectionStringName = "SqliteDb";
        private readonly ISqliteDataAccess _db;

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            throw new NotImplementedException();
        }

        public void CheckInGuest(int bookingId)
        {
            throw new NotImplementedException();
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sql = @"	SELECT rt.Id, rt.Title, rt.Description, rt.Price
	                        FROM Rooms r
	                        INNER JOIN RoomTypes rt ON rt.id = r.RoomTypeId
	                        WHERE r.Id NOT IN (
		                        SELECT b.RoomId 
		                        FROM Bookings b
		                        WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
			                        OR (b.StartDate <= @endDate AND @endDate < b.EndDate)
			                        OR (b.StartDate <= @startDate AND @startDate < b.EndDate)
		                        )
	                        GROUP BY rt.Id, rt.Title, rt.Description, rt.Price;";


            var output = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                        new { startDate, endDate },
                                                        connectionStringName);

            output.ForEach(x => x.Price = x.Price / 100);

            return output;
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
