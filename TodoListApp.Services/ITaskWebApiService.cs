using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITaskWebApiService
    {
        Task<IEnumerable<TaskModel>> GetTasks();
        Task<TaskModel> GetTaskById(int taskId);
        System.Threading.Tasks.Task AddTask(TaskModel task);
        System.Threading.Tasks.Task UpdateTask(TaskModel task);
        System.Threading.Tasks.Task DeleteTask(int taskId);
    }
}
