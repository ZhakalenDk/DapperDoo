using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oiski.School.Web.DapperDoo.Domain
{
    public class TaskAccess
    {
        public TaskAccess(IDapperDooContext context)
        {
            _context = context;
        }

        private readonly IDapperDooContext _context;

        public async Task<IEnumerable<TaskItem>> GetTasksAsync() =>
            await _context.LoadDataAsync<TaskItem, dynamic>("Task_Get_All");

        [Obsolete("Use GetTaskAsync instead", true)]
        public IReadOnlyList<TaskItem> GetTasks() =>
            _context.Tasks;

        public async Task<TaskItem> GetByID(Guid ID)
        {
            var results = await _context.LoadDataAsync<TaskItem, dynamic>("Task_Get_By_ID", new { TaskID = ID });

            return results.FirstOrDefault();
        }

        public async Task<bool> AddTaskAsync(TaskItem task)
        {
            var results = await _context
                .SaveDataAsync("Task_Add", new { TaskID = task.ID.ToString("N"), task.Title, task.Description });

            return results > 0;
        }

        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            var results = await _context
                .SaveDataAsync("Task_Update", new { TaskID = task.ID.ToString("N"), task.Title, task.Description });

            return results > 0;
        }

        public async Task<bool> DeleteTaskAsync(TaskItem task)
        {
            var results = await _context
                .SaveDataAsync("Task_Delete", new { TaskID = task.ID.ToString("N") });

            return results > 0;
        }
    }
}
