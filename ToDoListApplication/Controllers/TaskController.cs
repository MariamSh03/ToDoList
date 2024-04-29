using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.Services.Database;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("TodoLists")]
    public class TaskController : ControllerBase
    {
        private readonly TodoListDatabaseService _todoService;

        public TaskController(TodoListDatabaseService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("tasks/{taskId}")]
        public IActionResult GetTaskDetails(int taskId)
        {
            try
            {
                var todoLists = _todoService.GetTodoLists();

                if (todoLists == null)
                {
                    return NotFound($"Task with ID {taskId} not found");
                }

                var task = todoLists.SelectMany(todoList => todoList.Tasks).FirstOrDefault(t => t.Id == taskId);

                if (task == null)
                {
                    return NotFound($"Task with ID {taskId} not found");
                }

                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, $"An error occurred while retrieving task details: {ex.Message}");
            }
        }

        [HttpDelete("tasks/{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            try
            {
                _todoService.DeleteTask(taskId);
                return Ok("Task deleted successfully");
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

        [HttpPut("tasks/{taskId}")]
        public IActionResult EditTask(int taskId, [FromBody] TaskModel taskModel)
        {
            try
            {
                var task = new Services.Task
                {
                    Id = taskId,
                    Title = taskModel.Title,
                    Description = taskModel.Description,
                    CreationDate = taskModel.CreationDate,
                    DueDate = taskModel.DueDate,
                    Status = (Services.TaskStatus)taskModel.Status,
                    Assignee = taskModel.Assignee,
                    Tags = taskModel.Tags,
                    Comments = taskModel.Comments,
                };

                this._todoService.EditTask(taskId, task);

                return Ok("Task edited successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("tasks/{taskId}/status")]
        public IActionResult UpdateTaskStatus(int taskId, [FromBody] string newStatus)
        {
            try
            {
                // Get the task from the database
                var task = _todoService.GetTaskById(taskId);

                if (task == null)
                {
                    return NotFound($"Task with ID {taskId} not found");
                }

                // Update the task status directly
                task.Status = Enum.Parse<Services.TaskStatus>(newStatus);

                // Update the task in the database
                _todoService.UpdateTask(task);

                return Ok("Task status updated successfully");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, $"An error occurred while updating task status: {ex.Message}");
            }
        }

    }
}
