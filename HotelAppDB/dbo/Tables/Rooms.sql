﻿CREATE TABLE [dbo].[Rooms]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RoomTypeId] INT NOT NULL, 
    [RoomNumber] VARCHAR(10) NOT NULL
)
