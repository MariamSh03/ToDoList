using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using System.Diagnostics;
using TodoListApp.WebApp.Models;
using TodoListApp.Services;
using TodoListApp.Services.WebApi;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITodoListWebApiService _todoListWebApiService;
        private readonly ITaskWebApiService _taskWebApiService;

        public HomeController(ILogger<HomeController> logger, ITodoListWebApiService todoListService, ITaskWebApiService taskWebApiService)
        {
            _logger = logger;
            _todoListWebApiService = todoListService;
            _taskWebApiService = taskWebApiService;
        }

        public async Task<IActionResult> Index()
        {
            return SignInPage();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public async Task<IActionResult> TodoLists()
        {
            var todoLists = await _todoListWebApiService.GetTodoLists();
            var todoListsModel = todoLists.Select(todoList => new TodoListModel
            {
                Id = todoList.Id,
                Title = todoList.Title,
                Description = todoList.Description,
                Tasks = todoList.Tasks.Select(task => new TaskModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreationDate = task.CreationDate,
                    DueDate = task.DueDate,
                    Status = (WebApi.Models.TaskStatus)task.Status,
                    Assignee = task.Assignee,
                    Tags = task.Tags,
                    Comments = task.Comments
                }).ToList(),
            }).ToList();

            return this.View(todoListsModel);
        }

        public IActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTodoList(int todoListId)
        {
            try
            {
                await _todoListWebApiService.DeleteTodoList(todoListId);
                return RedirectToAction("TodoLists"); // Redirect to TodoLists action after successful deletion
            }
            catch (ArgumentException ex)
            {
                // Handle any exceptions that might occur during deletion
                _logger.LogError($"An error occurred while deleting todo list: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                await _taskWebApiService.DeleteTask(taskId);
                return RedirectToAction("TodoLists");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"An error occurred while deleting task: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskModel taskModel)
        {
            try
            {
                // Call the service method to add the task
                await _taskWebApiService.AddTask(taskModel);

                // Redirect to the TodoLists action after successful addition
                return RedirectToAction("TodoLists");
            }
            catch (ArgumentException ex)
            {
                // Handle any exceptions that might occur during task addition
                _logger.LogError($"An error occurred while adding a new task: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(int taskId)
        {
            try
            {
                await _taskWebApiService.DeleteTask(taskId);
                return RedirectToAction("TodoLists");
            }
            catch (ArgumentException ex)
            {
                // Handle any exceptions that might occur during deletion
                _logger.LogError($"An error occurred while deleting todo list: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        public IActionResult SignInPage()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
