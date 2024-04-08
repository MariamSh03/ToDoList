using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Services.WebApi;
public class TaskService : ITaskService
{
    public void AddTaskToTodoList(int todoListId, Task task)
    {
        throw new NotImplementedException();
    }

    public void DeleteTask(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task GetTaskById(int taskId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> GetTasksForTodoList(int todoListId)
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(Task task)
    {
        throw new NotImplementedException();
    }
}
