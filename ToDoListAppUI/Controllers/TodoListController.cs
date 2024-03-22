using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoListApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            this.todoListService = todoListService;
        }

        // Define controller actions here...
    }
}
