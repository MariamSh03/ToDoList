using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITaskWebApiService
    {
        Task<IEnumerable<TaskModel>> GetTasks();
        Task<TaskModel> GetTaskById(int taskId);
        System.Threading.Tasks.Task AddTask(TaskModel task, int listId);
        System.Threading.Tasks.Task UpdateTask(TaskModel task);
        System.Threading.Tasks.Task DeleteTask(int taskId);
        System.Threading.Tasks.Task UpdateTaskStatus(int taskId, TaskStatus status);
    }
}
