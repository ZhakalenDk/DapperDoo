/*
Author(s):
	Mike Mortensen
Description:
	Gets the entire Task collection
Paremeters:
	None
Returns:
	A range of Task records
*/

CREATE PROCEDURE [dbo].[Task_Get_All]
AS
BEGIN
	SELECT * FROM dbo.[Tasks]
END
