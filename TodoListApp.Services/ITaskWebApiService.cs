namespace TodoListApp.Services;
public interface ITaskWebApiService
{
    public interface ITaskWebApiService
    {
        Task<IEnumerable<Task>> GetTasks();
        Task AddTask(Task task);
        Task DeleteTask(int taskId);
        Task UpdateTask(Task task);
        Task<Task> GetTaskById(int taskId);
    }
}
