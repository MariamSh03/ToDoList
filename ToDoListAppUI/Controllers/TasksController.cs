using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITodoListService _todoListService;
        private readonly ITaskService _taskService;

        public TasksController(ITodoListService todoListService, ITaskService taskService)
        {
            _todoListService = todoListService;
            _taskService = taskService;
        }

        // Action method to view tasks for a specific to-do list
        public IActionResult Index(int todoListId)
        {
            // Retrieve the to-do list with the specified ID
            var todoList = _todoListService.GetTodoListById(todoListId);

            if (todoList == null)
            {
                return NotFound(); // Return 404 if the to-do list is not found
            }

            // Retrieve the tasks for the specified to-do list
            var tasks = _taskService.GetTasksForTodoList(todoListId);

            // Assign tasks to the TodoList.Tasks property
            todoList.Tasks = (List<Services.Task>?)tasks;

            return View(todoList); // Pass the modified TodoList object to the view
        }

        // Other action methods for adding, editing, or deleting tasks can be added here
    }
}
