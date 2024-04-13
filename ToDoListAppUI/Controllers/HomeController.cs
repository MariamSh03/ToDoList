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

        public HomeController(ILogger<HomeController> logger, ITodoListWebApiService todoListService)
        {
            _logger = logger;
            _todoListWebApiService = todoListService;
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
