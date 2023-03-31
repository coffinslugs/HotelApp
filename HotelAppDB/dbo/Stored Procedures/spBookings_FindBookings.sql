CREATE PROCEDURE [dbo].[spBookings_FindBookings]
	@lastName nvarchar,
	@startDate date
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[CheckedIn], [b].[TotalCost],
		[g].[FirstName], [g].[LastName],
		[r].[RoomTypeId], [r].[RoomNumber],
		[rt].[Title], [rt].[Description], [rt].[Price]
	FROM dbo.Bookings b
		INNER JOIN dbo.Guests g ON b.GuestId = g.Id
		INNER JOIN dbo.Rooms r ON b.RoomId = r.Id
		INNER JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.Id
	WHERE b.StartDate = @startDate
		AND g.LastName = @lastName
		AND b.CheckedIn = 0;
END