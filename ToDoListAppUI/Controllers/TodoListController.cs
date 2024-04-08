using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

public class TodoListController : Controller
{
    private readonly ITodoListService _todoListService;

    public TodoListController(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    public async Task<IActionResult> Index()
    {
        var todoLists = this._todoListService.GetTodoLists();

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
        });

        return View(todoListModels);
    }
}
