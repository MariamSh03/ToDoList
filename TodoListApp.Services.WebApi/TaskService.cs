using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TaskService : ITaskService
    {
        private readonly ITaskService _taskService;

        public TaskService(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public void AddTaskToTodoList(int todoListId, Task task)
        {
            _taskService.AddTaskToTodoList(todoListId, task);
        }

        public void DeleteTask(int taskId)
        {
            _taskService.DeleteTask(taskId);
        }

        public Task GetTaskById(int taskId)
        {
            return _taskService.GetTaskById(taskId);
        }

        public IEnumerable<Task> GetTasksForTodoList(int todoListId)
        {
            return _taskService.GetTasksForTodoList(todoListId);
        }

        public void UpdateTask(Task task)
        {
            _taskService.UpdateTask(task);
        }
    }
}
