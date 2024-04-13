using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.Services.Database;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TodoListDatabaseService _todoService;

        public TaskController(TodoListDatabaseService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("{todoListId}/tasks/{taskId}")]
        public IActionResult GetTaskDetails(int todoListId, int taskId)
        {
            try
            {
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
}
