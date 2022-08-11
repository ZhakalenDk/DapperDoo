CREATE TABLE [dbo].[Tasks]
(
	[TaskID] varchar(32) NOT NULL PRIMARY KEY,
	[Title] nvarchar(25) NOT NULL,
	[Description] nvarchar(150)
)
