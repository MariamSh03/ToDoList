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

    [HttpGet("{todoListId}/tasks/{taskId}")]
    public IActionResult GetTaskDetails(int todoListId, int taskId)
    {
        try
        {
            // Retrieve the specific todo list
            var todoList = _todoService.GetTodoListById(todoListId);
            if (todoList == null)
            {
                return NotFound($"Todo list with ID {todoListId} not found");
            }

            var task = todoList.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound($"Task with ID {taskId} not found in todo list with ID {todoListId}");
            }

            return Ok(task);
        }
        catch (ArgumentException ex)
        {
            return StatusCode(500, $"An error occurred while retrieving task details: {ex.Message}");
        }
    }

    [HttpDelete("{todoListId}/tasks/{taskId}")]
    public IActionResult DeleteTask(int todoListId, int taskId)
    {
        try
        {
            _todoService.DeleteTask(todoListId, taskId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{todoListId}/tasks")]
    public IActionResult AddTask(int todoListId, [FromBody] TaskModel taskModel)
    {
        try
        {
            var task = new TodoListApp.Services.Task
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                CreationDate = taskModel.CreationDate,
                DueDate = taskModel.DueDate,
                Status = (TodoListApp.Services.TaskStatus)taskModel.Status,
                Assignee = taskModel.Assignee,
                Tags = taskModel.Tags,
                Comments = taskModel.Comments
            };

            _todoService.AddTask(todoListId, task);

            return Ok("Task added successfully");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{todoListId}/tasks/{taskId}")]
    public IActionResult EditTask(int todoListId, int taskId, [FromBody] TaskModel taskModel)
    {
        try
        {
            var task = new TodoListApp.Services.Task
            {
                Id = taskId, // Set the ID of the task being edited
                Title = taskModel.Title,
                Description = taskModel.Description,
                CreationDate = taskModel.CreationDate,
                DueDate = taskModel.DueDate,
                Status = (TodoListApp.Services.TaskStatus)taskModel.Status,
                Assignee = taskModel.Assignee,
                Tags = taskModel.Tags,
                Comments = taskModel.Comments
            };

            _todoService.EditTask(todoListId, task);

            return Ok("Task edited successfully");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
