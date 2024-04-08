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

        public IActionResult Index(int todoListId)
        {
            var todoList = _todoListService.GetTodoListById(todoListId);

            if (todoList == null)
            {
                return NotFound();
            }

            var tasks = _taskService.GetTasksForTodoList(todoListId);

            todoList.Tasks = (List<Services.Task>?)tasks;

            return View(todoList);
        }

    }
}
