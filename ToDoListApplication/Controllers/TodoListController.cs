using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Database;
using TodoListApp.WebApi.Models;

[ApiController]
[Route("[controller]")]
public class TodoListController : ControllerBase
{
    private readonly TodoListDatabaseService _todoService;

    public TodoListController(TodoListDatabaseService todoService)
    {
        this._todoService = todoService;
    }

    [HttpGet]
    [Produces("application/json")]
    public IActionResult GetTodoLists()
    {
        var todoLists = _todoService.GetTodoLists();
        var todoListModels = todoLists.Select(todo => new TodoListModel
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted

            // Map other properties as needed
        }).ToList();

        return Ok(todoListModels);
    }
}
