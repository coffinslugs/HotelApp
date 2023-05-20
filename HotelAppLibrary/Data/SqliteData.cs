using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string sql = @"SELECT 1 FROM Guests WHERE FirstName = @firstName AND LastName = @lastName";
            int results = _db.LoadData<dynamic, dynamic>(sql,
                                                         new { firstName, lastName },
                                                         connectionStringName).Count();

            if (results == 0)
            {
                sql = @"INSERT INTO Guests (FirstName, LastName)
		                VALUES (@firstName, @lastName);";

                _db.SaveData(sql,
                             new { firstName, lastName },
                             connectionStringName);
            }

            sql = @"SELECT [Id], [FirstName], [LastName]
	                    FROM Guests
	                    WHERE FirstName = @firstName AND LastName = @lastName LIMIT 1;";

            // Create Guest
            GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql,
                                                                 new { firstName, lastName },
                                                                 connectionStringName).First();
            // Get Room Type
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from RoomTypes where Id = @Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName).First();
            // Calculate how long they want to stay
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            sql = @"SELECT r.*
	                FROM Rooms r
	                INNER JOIN RoomTypes rt ON rt.Id = r.RoomTypeId
	                WHERE r.RoomTypeId = @roomTypeId
		                AND r.Id NOT IN (
		                SELECT b.RoomId 
		                FROM Bookings b
		                WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
			                OR (b.StartDate <= @endDate AND @endDate < b.EndDate)
			                OR (b.StartDate <= @startDate AND @startDate < b.EndDate)
		                );";

            // Get available rooms for date range selected
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql,
                                                                              new { startDate, endDate, roomTypeId },
                                                                              connectionStringName);

            sql = @"INSERT INTO Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
	                VALUES (@roomId, @guestId, @startDate, @endDate, @totalCost);";

            // Create Booking
            _db.SaveData(sql,
                         new
                         {
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             totalCost = timeStaying.Days * roomType.Price
                         },
                         connectionStringName);
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
            string sql = @"SELECT [Id], [Title], [Description], [Price]
	                    FROM RoomTypes
	                    Where Id = @id;";

            return _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                        new { id },
                                                        connectionStringName).FirstOrDefault();
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
