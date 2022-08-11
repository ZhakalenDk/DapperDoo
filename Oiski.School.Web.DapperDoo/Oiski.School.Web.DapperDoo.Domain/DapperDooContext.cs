using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace Oiski.School.Web.DapperDoo.Domain
{
    /// <summary>
    /// Represents the closest relationship between the <strong>SQL</strong> backend store and the program
    /// </summary>
    public class DapperDooContext : IDapperDooContext
    {
        public DapperDooContext(IConfiguration config)
        {
            _config = config;
        }

        private readonly IConfiguration _config;

        /// <summary>
        /// <strong>Test subject</strong>. Introduces risk for thread deadlock - DO NOT USE
        /// </summary>
        public IReadOnlyList<TaskItem> Tasks
        {
            get => LoadDataAsync<TaskItem, dynamic>("Task_Get_All")
                .GetAwaiter()
                .GetResult()
                .ToList();
        }

        /// <summary>
        /// Retrieves an <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> from the <strong>SQL</strong> backend store
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to map to</typeparam>
        /// <typeparam name="TPara">Defines the type of the parameter(s)</typeparam>
        /// <param name="storedProcedure">The procedure to be executed</param>
        /// <param name="parameters">Defines the set of parameters needed to execute <paramref name="storedProcedure"/></param>
        /// <param name="connectionID">The configuration ID for the connection string that should be used. (<i>Default is "Default"</i>)</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <typeparamref name="TEntity"/> containing the result of <paramref name="storedProcedure"/></returns>
        public async Task<IEnumerable<TEntity>> LoadDataAsync<TEntity, TPara>(string storedProcedure, TPara parameters = default, string connectionID = "Default")
        {
            using IDbConnection conn = new SqlConnection(_config.GetConnectionString(connectionID));

            return await conn.QueryAsync<TEntity>($"dbo.{storedProcedure}", parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Pushes <paramref name="parameters"/> to the <strong>SQL</strong> backend store
        /// </summary>
        /// <typeparam name="TPara">The type of the parameter(s)</typeparam>
        /// <param name="storedProcedure">The procedure to be executed</param>
        /// <param name="parameters">Defines the set of parameters needed to execute <paramref name="storedProcedure"/></param>
        /// <param name="connectionID">The configuration ID for the connection string that should be used. (<i>Default is "Default"</i>)</param>
        /// <returns>The amount of row that were affected by the execution</returns>
        public async Task<int> SaveDataAsync<TPara>(string storedProcedure, TPara parameters = default, string connectionID = "Default")
        {
            using IDbConnection conn = new SqlConnection(_config.GetConnectionString(connectionID));

            return await conn.ExecuteAsync($"dbo.{storedProcedure}", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
