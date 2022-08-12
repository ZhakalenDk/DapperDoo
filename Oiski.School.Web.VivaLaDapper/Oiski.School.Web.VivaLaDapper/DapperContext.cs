using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations;
using static Dapper.SqlMapper;

namespace Oiski.School.Web.VivaLaDaper
{
    /// <summary>
    /// Defines the <see langword="base"/> for a <i>generic</i> <strong>SQL</strong> connection
    /// </summary>
    public abstract class DapperContext
    {
        public DapperContext(IConfiguration config)
        {
            _config = config;
        }

        private readonly IConfiguration? _config;

        /// <summary>
        /// Adds <paramref name="entity"/> to the <strong>SQL</strong> backend store
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="connectionID"></param>
        /// <returns><see langword="True"/> if <paramref name="entity"/> could be added; otherwise, if not, <see langword="false"/></returns>
        public async Task<bool> AddAsync<TEntity>(TEntity entity, string connectionID = "Default") where TEntity : class
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));

            string? tableName = GetTableData(entity).Item2;

            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            StringBuilder ColumnBuilder = new StringBuilder();
            StringBuilder valueBuilder = new StringBuilder();
            for (int i = 0; i < properties.Length; i++) //  Build column and value list
            {
                var property = properties[i];
                string comma = ((i < properties.Length - 1) ? (",") : (string.Empty));  //  Only append a comma if it's not the last property
                ColumnBuilder.Append($"{property.Name}{comma}");

                object? propertyValue = property.GetValue(entity);
                bool isString = propertyValue.GetType().IsAssignableFrom(typeof(string));   //  In case it's a string an additonal setup is needed

                valueBuilder.Append($"{((isString) ? ($"'{propertyValue}'") : (propertyValue))}{comma}");
            }

            string colums = ColumnBuilder.ToString();
            string values = valueBuilder.ToString();

            string insertQuery = $"INSERT INTO {tableName} ({colums}) VALUES ({values})";

            return (await connection.ExecuteAsync(insertQuery)) > 0;
        }

        /// <summary>
        /// Update <paramref name="entity"/> on the <strong>SQL</strong> backend store
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="connectionID"></param>
        /// <returns><see langword="True"/> if <paramref name="entity"/> could be updated; otherwise, if not, <see langword="false"/></returns>
        public async Task<bool> UpdateAsync<TEntity>(TEntity entity, string connectionID = "Default") where TEntity : class
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));

            string? tableName = GetTableData(entity).Item2;

            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            StringBuilder builder = new StringBuilder();
            KeyValuePair<string?, object?>? key = null;
            for (int i = 0; i < properties.Length; i++) //  Build column and value list
            {
                var property = properties[i];
                string? propertyName = property.Name;
                object? propertyValue = property.GetValue(entity);

                KeyAttribute? keyAttribute = ( KeyAttribute? )Attribute.GetCustomAttribute(property, typeof(KeyAttribute));   //  Getting the key if the attribute is present

                if (keyAttribute != null)
                {
                    key = new KeyValuePair<string?, object?>(propertyName, propertyValue);
                }
                else
                {
                    string comma = ((i < properties.Length - 1) ? (",") : (string.Empty));  //  Only append a comma if it's not the last property
                    bool isString = propertyValue.GetType().IsAssignableFrom(typeof(string));   //  In case it's a string an additonal setup is needed

                    builder.Append($"{property.Name} = {((isString) ? ($"'{propertyValue}'") : (propertyValue))}{comma}");
                }
            }

            string colums = builder.ToString();

            string insertQuery = $"UPDATE {tableName} SET {colums} WHERE ({key?.Key} = {key?.Value})";

            return (await connection.ExecuteAsync(insertQuery)) > 0;
        }

        /// <summary>
        /// Remove <paramref name="entity"/> from the <strong>SQL</strong> backend store
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="connectionID"></param>
        /// <returns><see langword="True"/> if <paramref name="entity"/> could be deleted; otherwise, if not, <see langword="false"/></returns>
        public async Task<bool> RemoveAsync<TEntity>(TEntity entity, string connectionID = "Default") where TEntity : class
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));

            string? tableName = GetTableData(entity).Item2;

            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            KeyValuePair<string?, object?>? key = null;
            for (int i = 0; i < properties.Length; i++) //  Build column and value list
            {
                var property = properties[i];
                string? propertyName = property.Name;
                object? propertyValue = property.GetValue(entity);

                KeyAttribute? keyAttribute = ( KeyAttribute? )Attribute.GetCustomAttribute(property, typeof(KeyAttribute));

                if (keyAttribute != null)
                {
                    key = new KeyValuePair<string?, object?>(propertyName, propertyValue);
                }
            }

            string insertQuery = $"DELETE FROM {tableName} WHERE ({key?.Key} = {key?.Value})";

            return (await connection.ExecuteAsync(insertQuery)) > 0;
        }

        public async Task<TEntity> GetByKeyAsync<TEntity, TKey>(TKey key, string connectionID = "Default") where TEntity : class, new()
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));

            string? tableName = GetTableData(new TEntity()).Item2;

            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            string? keyPropertyName = null;
            for (int i = 0; i < properties.Length; i++) //  Build column and value list
            {
                var property = properties[i];
                string? propertyName = property.Name;

                KeyAttribute? keyAttribute = ( KeyAttribute? )Attribute.GetCustomAttribute(property, typeof(KeyAttribute));

                if (keyAttribute != null)
                {
                    keyPropertyName = propertyName;
                }
            }

            return await connection.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM {tableName} WHERE ({keyPropertyName} = {key})");
        }

        public async Task<IEnumerable<TEntity>> GetAll<TEntity>(string connectionID = "Default") where TEntity : class, new()
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));

            string? tableName = GetTableData(new TEntity()).Item2;

            return await connection.QueryAsync<TEntity>($"SELECT * FROM {tableName}");
        }

        /// <summary>
        /// Gets the <strong>name</strong> and the <strong>schema</strong> of the associated table to <paramref name="entity"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns>(TableSchema, TableName)</returns>
        private (string?, string?) GetTableData<TEntity>(TEntity entity)
        {
            TableAttribute? tableAttribute = ( TableAttribute? )Attribute.GetCustomAttribute(typeof(TEntity), typeof(TableAttribute));

            var tableSchema = tableAttribute?.Schema;
            var tableName = tableAttribute?.Name;

            return (tableSchema, tableName);
        }
    }
}
