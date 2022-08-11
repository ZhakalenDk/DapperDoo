namespace Oiski.School.Web.DapperDoo.Domain
{
    public interface IDapperDooContext
    {
        IReadOnlyList<TaskItem> Tasks { get; }

        Task<IEnumerable<TEntity>> LoadDataAsync<TEntity, TPara>(string storedProcedure, TPara paramaters = default, string connectionID = "Default");
        Task<int> SaveDataAsync<TPara>(string storedProcedure, TPara parameters = default, string connectionID = "Default");
    }
}