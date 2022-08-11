/*
Author(s):
	Mike Mortensen
Description:
	Deletes the Task where the ID matches the passed @TaskID
Paremeters:
	@TaskID: The ID to look for (Intended to be a GUID)
Returns:
	The count of affected rows, which in this case would be either 1 or 0
*/

CREATE PROCEDURE [dbo].[Task_Delete]
	@TaskID varchar(32)
AS
BEGIN
	DECLARE @affectedRows AS int = 0;

	IF (EXISTS (SELECT * FROM dbo.[Tasks] WHERE (TaskID = @TaskID)))
		DELETE dbo.[Tasks]
		WHERE (TaskID = @TaskID)
		SET @affectedRows += @@ROWCOUNT

RETURN @affectedRows
END
