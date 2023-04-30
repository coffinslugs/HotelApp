﻿CREATE PROCEDURE [dbo].[spRoomTypes_GetById]
	@id int
AS
BEGIN 
	SET NOCOUNT ON;

	SELECT [Id], [Title], [Description], [Price]
	FROM dbo.RoomTypes
	Where Id = @id;

END