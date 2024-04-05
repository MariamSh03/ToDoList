using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : Controller
    {
        private readonly ITodoListService todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            this.todoListService = todoListService;
        }

        public IActionResult Index()
        {
            var todoLists = todoListService.GetTodoLists();
            return this.View(todoLists);
        }

    }
}
