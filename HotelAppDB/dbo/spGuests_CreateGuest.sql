CREATE PROCEDURE [dbo].[spGuests_CreateGuest]
	@firstName nvarchar,
	@lastName nvarchar
AS
BEGIN
	set nocount on;

	INSERT INTO dbo.Guests(FirstName, LastName)
	VALUES (@firstName, @lastName);

END

RETURN id;