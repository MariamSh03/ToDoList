using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
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
        var todoLists = this._todoService.GetTodoLists();
        var todoListModels = todoLists.Select(todo => new TodoListModel
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted,

            // Map other properties as needed
        }).ToList();

        return this.Ok(todoListModels);
    }

    [HttpPost]
    public IActionResult AddTodoList([FromBody] TodoList todoList)
    {
        if (todoList == null)
        {
            return this.BadRequest("TodoList object is null");
        }

        this._todoService.AddTodoList(todoList);
        return this.Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTodoList(int id)
    {
        try
        {
            this._todoService.DeleteTodoList(id);
            return this.Ok();
        }
        catch (ArgumentException ex)
        {
            return this.NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTodoList(int id, [FromBody] TodoListModel updatedTodoList)
    {
        try
        {
            this._todoService.UpdateTodoList(id, new TodoList
            {
                Title = updatedTodoList.Title,
                IsCompleted = updatedTodoList.IsCompleted,

                // Map other properties as needed
            });

            return this.Ok();
        }
        catch (ArgumentException ex)
        {
            return this.NotFound(ex.Message); // Return 404 Not Found if the to-do list is not found
        }
    }
}
