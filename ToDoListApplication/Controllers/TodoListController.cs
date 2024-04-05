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
        var todoLists = this._todoService.GetTodoLists().ToList();
        var todoListModels = todoLists.Select(todo => new TodoListModel
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Tasks = todo.Tasks.Select(task => new TaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate,
                Tags = task.Tags,
                Comments = task.Comments,
                DueDate = task.DueDate,
                Status = (TodoListApp.WebApi.Models.TaskStatus)task.Status,
                Assignee = task.Assignee,
            }).ToList(),
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
    public IActionResult UpdateTodoList(int id, [FromBody] TodoList updatedTodoList)
    {
        try
        {
            this._todoService.UpdateTodoList(id, updatedTodoList);
            return this.Ok();
        }
        catch (ArgumentException ex)
        {
            return this.NotFound(ex.Message);
        }
    }

    [HttpGet("{todoListId}")]
    public IActionResult GetTodoWithId(int todoListId)
    {
        try
        {
            var todoList = _todoService.GetTodoListById(todoListId);
            if (todoList == null)
            {
                return NotFound($"Todo list with ID {todoListId} not found");
            }

            return this.Ok(todoList);
        }
        catch(ArgumentException ex)
        {
            return StatusCode(500, $"An error occurred while retrieving ToDOlistbyid details: {ex.Message}");
        }
    }
}
