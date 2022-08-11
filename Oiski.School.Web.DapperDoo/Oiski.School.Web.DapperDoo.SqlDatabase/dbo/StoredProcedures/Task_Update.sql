/*
Author(s):
	Mike Mortensen
Description:
	Updates the Task where the ID matches the passed @TaskID
Paremeters:
	@TaskID: The ID to look for (Intended to be a GUID)
Returns:
	The count of affected rows, which in this case would be either 1 or 0
*/

CREATE PROCEDURE [dbo].[Task_Update]
	@TaskID varchar(32),
	@Title nvarchar(25),
	@Description nvarchar(150)
AS
BEGIN
	DECLARE @affectedRows AS int = 0;

	IF (EXISTS (SELECT * FROM dbo.[Tasks] WHERE (TaskID = @TaskID)))
		UPDATE dbo.[Tasks] SET Title = @Title, Description = @Description
		WHERE (TaskID = @TaskID)
		SET @affectedRows += @@ROWCOUNT

RETURN @affectedRows
END
