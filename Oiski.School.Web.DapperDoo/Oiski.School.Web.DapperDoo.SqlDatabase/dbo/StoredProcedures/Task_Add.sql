/*
Author(s):
	Mike Mortensen
Description:
	Adds a Task record unless a Task with the same ID already exists.
Paremeters:
	@TaskID: The ID of the Task to insert (Intended to be a GUID)
	@Title:	The title of the Task to insert (Supports Unicode)
	@Description: THe description of the Task to insert (Supports Unicode)
Returns:
	The count of the affected rows, which in this case is 1 or 0
*/

CREATE PROCEDURE [dbo].[Task_Add]
	@TaskID varchar(32),
	@Title nvarchar(25),
	@Description nvarchar(150)
AS
BEGIN
		DECLARE @affectedRows AS int = 0;

		IF ((SELECT COUNT (1) FROM dbo.[Tasks] WHERE (TaskID = @TaskID)) = 0)	--	Ensuring no other Task with the same ID exists
			INSERT INTO dbo.[Tasks] (TaskID, Title, Description)
			VALUES (@TaskID, @Title, @Description)
			SET @affectedRows += @@ROWCOUNT

	RETURN @affectedRows
END
