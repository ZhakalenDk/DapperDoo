/*
Author(s):
	Mike Mortensen
Description:
	Gets the Task where the ID matches the passed @TaskID
Paremeters:
	@TaskID: The ID to look for (Intended to be a GUID)
Returns:
	The Task record if one was found, othewise, if none were found, NULL
*/

CREATE PROCEDURE [dbo].[Task_Get_By_ID]
	@TaskID varchar(32)
AS
BEGIN
	IF (EXISTS (SELECT * FROM dbo.[Tasks] WHERE (TaskID = @TaskID)))
		RETURN SELECT * FROM dbo.[Tasks] WHERE (TaskID = @TaskID)

	RETURN NULL
END
