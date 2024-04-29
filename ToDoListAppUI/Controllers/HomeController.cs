using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Models;

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

        public async Task<IActionResult> TodoLists(string searchString)
        {
            // Get all todo lists
            var todoLists = await _todoListWebApiService.GetTodoLists();

            // Filter todo lists based on search string
            if (!string.IsNullOrEmpty(searchString))
            {
                todoLists = todoLists.Where(t => t.Title.Contains(searchString) ||
                                                  t.Tasks.Any(task => task.Title.Contains(searchString)));
            }

            // Map to ViewModel
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

            // Pass search string to the view
            ViewBag.SearchString = searchString;

            return this.View(todoListsModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTodoList(int todoListId)
        {
            try
            {
                await _todoListWebApiService.DeleteTodoList(todoListId);
                return RedirectToAction("TodoLists");
            }
            catch (ArgumentException ex)
            {
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
        public async Task<IActionResult> AddTask(TaskModel taskModel, int listId)
        {
            try
            {
                DateTime dueDate = DateTime.ParseExact(taskModel.DueDate.ToString("yyyy-MM-ddTHH:mm:ss"), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                taskModel.DueDate = dueDate;
                taskModel.Assignee= listId.ToString();
                taskModel.Comments = string.Empty;
                // Call the service method to add the task
                await _taskWebApiService.AddTask(taskModel, listId);

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
        public async Task<IActionResult> AddTodo(TodoListModel newList)
        {
            try
            {
                var list = new TodoList
                {
                    Id = newList.Id,
                    Title = newList.Title,
                    Description = newList.Description,
                    Tasks = new List<Services.Task>(),
                };

                await _todoListWebApiService.AddTodoList(list);

                return RedirectToAction("TodoLists");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"An error occurred while adding a new TodoLIst: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(TaskModel taskModel)
        {
            try
            {
                DateTime dueDate = DateTime.ParseExact(taskModel.DueDate.ToString("yyyy-MM-ddTHH:mm:ss"), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                taskModel.DueDate = dueDate;
                taskModel.Assignee = "me";
                taskModel.Comments = string.Empty;
                await _taskWebApiService.UpdateTask(taskModel);

                return RedirectToAction("TodoLists");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"An error occurred while updating the task: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> GetTaskDetails(int taskId)
        {
            try
            {
                // Get the task details by its ID
                var taskDetails = await _taskWebApiService.GetTaskById(taskId);

                // Check if the task details are retrieved successfully
                if (taskDetails != null)
                {
                    // Pass the task details to the view
                    return View(taskDetails);
                }
                else
                {
                    // Task with the provided ID not found
                    _logger.LogWarning($"Task with ID {taskId} not found.");
                    return RedirectToAction("Error", new { message = "Task not found." });
                }
            }
            catch (ArgumentException ex)
            {
                // Handle any exceptions that might occur
                _logger.LogError($"An error occurred while fetching task details: {ex.Message}");
                return RedirectToAction("Error", new { message = "An error occurred while fetching task details." });
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
