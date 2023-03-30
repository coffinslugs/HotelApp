CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate date,
	@endDate date
AS
BEGIN
	set nocount on;

	SELECT rt.Id, rt.Title, rt.Description, rt.Price
	FROM dbo.Rooms r
	INNER JOIN dbo.RoomTypes rt ON rt.id = r.RoomTypeId
	WHERE r.Id NOT IN (
		SELECT b.RoomId 
		FROM dbo.Bookings b
		WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
			OR (b.StartDate <= @endDate AND @endDate < b.EndDate)
			OR (b.StartDate <= @startDate AND @startDate < b.EndDate)
		)
	GROUP BY rt.Id, rt.Title, rt.Description, rt.Price;
END
