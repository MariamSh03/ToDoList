namespace TodoListApp.Services
{
    public interface ITaskService
    {
        IEnumerable<Task> GetTasksForTodoList(int todoListId);
        Task GetTaskById(int taskId);
        void AddTaskToTodoList(int todoListId, Task task);
        void UpdateTask(Task task);
        void DeleteTask(int taskId);
    }
}
