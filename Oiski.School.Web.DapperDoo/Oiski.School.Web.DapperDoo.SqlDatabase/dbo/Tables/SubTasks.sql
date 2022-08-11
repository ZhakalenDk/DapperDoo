CREATE TABLE [dbo].[SubTasks]
(
	[SubTaskID] varchar(32) NOT NULL PRIMARY KEY,
	[Title] nvarchar(25) NOT NULL,

	[TaskID] varchar(32) NOT NULL,

	FOREIGN KEY(TaskID) REFERENCES [dbo].[Tasks](TaskID)
)
