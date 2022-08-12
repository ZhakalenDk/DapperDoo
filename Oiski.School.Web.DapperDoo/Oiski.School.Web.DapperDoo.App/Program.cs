using System.Data;
using System.Data.SqlClient;
using Dapper;
using Oiski.School.Web.DapperDoo.App;

using IDbConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Database=SampleDatabase; Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False"); //  Used by all samples

#region Dapper Execute Sample
var rows = await connection.ExecuteAsync("INSERT INTO Samples (Title) values (@Title)", new { Title = "MySampleTitle" });
rows += await connection.ExecuteAsync("INSERT INTO Samples2 (Title) values (@Title)", new { Title = "MySampleTitle2" });

Console.WriteLine($"Affected Rows: {rows}");
#endregion

#region Dapper Query Sample
var queryEntity = (await connection.QueryAsync<SampleEntity>("SELECT * FROM Samples"))
    .FirstOrDefault();

Console.WriteLine($"Sample (Query): {queryEntity.Title}");
#endregion

#region Dapper QueryFirst Sample
var queryFirstEntity = (await connection.QueryFirstAsync<SampleEntity>("SELECT * FROM Samples"));

Console.WriteLine($"Sample (Query First): {queryFirstEntity.Title}");
#endregion

#region Dapper QueryFirstOrDefault Sample
var queryFirstOrDefaultEntity = (await connection.QueryFirstOrDefaultAsync<SampleEntity>("SELECT * FROM Samples"));

Console.WriteLine($"Sample (Query Single): {queryFirstOrDefaultEntity.Title}");
#endregion

#region Dapper QuerySingle Sample
var querySingleEntity = (await connection.QuerySingleAsync<SampleEntity>("SELECT * FROM Samples"));

Console.WriteLine($"Sample (Query Single): {querySingleEntity.Title}");
#endregion

#region Dapper QuerySingleOrDefault Sample
var querySingleOrDefaultEntity = (await connection.QuerySingleOrDefaultAsync<SampleEntity>("SELECT * FROM Samples"));

Console.WriteLine($"Sample (Query Single or Default): {querySingleOrDefaultEntity.Title}");
#endregion

#region Dapper Multiple Sample
using var multiQuery = await connection.QueryMultipleAsync("SELECT * FROM Samples; SELECT * FROM Samples2");
var sampleEntity = multiQuery.Read<SampleEntity>()
    .FirstOrDefault();

var sampleEntity2 = multiQuery.Read<SampleEntity2>()
    .FirstOrDefault();

Console.WriteLine($"Sample (Query Multiple (SampleEntity)): {sampleEntity.Title}");
Console.WriteLine($"Sample (Query Multiple (SampleEntity2)): {sampleEntity2.Title}");
#endregion

#region Clean up crew
var DeleteRows = await connection.ExecuteAsync("DELETE FROM Samples WHERE (Title = @Title)", new { Title = "MySampleTitle" });
DeleteRows += await connection.ExecuteAsync("DELETE FROM Samples2 WHERE (Title =  @Title)", new { Title = "MySampleTitle2" });

Console.WriteLine($"Deleted Rows: {rows}");
#endregion