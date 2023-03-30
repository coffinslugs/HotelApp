CREATE PROCEDURE [dbo].[spGuests_CreateGuest]
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
BEGIN
	set nocount on;

	IF NOT EXISTS (SELECT 1 FROM dbo.Guests WHERE FirstName = @firstName AND LastName = @lastName)
	BEGIN
		INSERT INTO dbo.Guests(FirstName, LastName)
		VALUES (@firstName, @lastName);
	END

	SELECT TOP 1 [Id], [FirstName], [LastName]
	FROM dbo.Guests
	WHERE FirstName = @firstName AND LastName = @lastName;

END
