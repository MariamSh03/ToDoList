using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TodoListApp.Services;

namespace TodoListApp.Services.Database;
public class TodoListDatabaseService : ITodoListService
{
    private readonly TodoListDbContext dbContext_;

    public TodoListDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext_ = dbContext;
    }
    public void AddTodoList(TodoList todoList)
    {
        if (todoList == null)
        {
            throw new ArgumentNullException(nameof(todoList));
        }

        var newTodoListEntity = new TodoListEntity
        {
            Title = todoList.Title,
            Description = todoList.Description,
            Tasks = todoList.Tasks.Select(task => new TaskEntity
            {
                // Map properties from Task to TaskEntity
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate,
                DueDate = task.DueDate,
                Status = (Database.TaskStatus)task.Status,
                Assignee = task.Assignee,
                Tags = task.Tags,
                Comments = task.Comments
            }).ToList()
        };

        _ = this.dbContext_.TodoLists.Add(newTodoListEntity);
        _ = this.dbContext_.SaveChanges();
    }

    public void DeleteTodoList(int todoListId)
    {
        var todoListEntity = this.dbContext_.TodoLists.Find(todoListId);

        if (todoListEntity == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }

        _ = this.dbContext_.TodoLists.Remove(todoListEntity);
        _ = this.dbContext_.SaveChanges();
    }

    public void UpdateTodoList(int todoListId, TodoList updatedTodoList)
    {
        var existingTodoList = this.dbContext_.TodoLists.Find(todoListId);
        if (existingTodoList == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }

        // Update the properties of the existing to-do list
        existingTodoList.Title = updatedTodoList.Title;
        existingTodoList.Description = updatedTodoList.Description;
        existingTodoList.Tasks = updatedTodoList.Tasks.Select(task => new TaskEntity
        {
            // Map properties from Task to TaskEntity
            Title = task.Title,
            Description = task.Description,
            CreationDate = task.CreationDate,
            DueDate = task.DueDate,
            Status = (Database.TaskStatus)task.Status,
            Assignee = task.Assignee,
            Tags = task.Tags,
            Comments = task.Comments
        }).ToList();
        // Update other properties as needed

        _ = this.dbContext_.SaveChanges();
    }

    IEnumerable<TodoList> ITodoListService.GetTodoLists()
    {
        var todoListEntities = this.dbContext_.TodoLists.ToList();
        var todoLists = todoListEntities.Select(entity => new TodoList
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
            // Map other properties as needed
        }).ToList();

        return todoLists;
    }

    TodoList ITodoListService.GetTodoListById(int todoListId)
    {
        throw new NotImplementedException();
    }

    void ITodoListService.AddTodoList(TodoList todoList)
    {
        if (todoList == null)
        {
            throw new ArgumentNullException(nameof(todoList));
        }

        var newTodoListEntity = new TodoListEntity
        {
            Title = todoList.Title,
            Description = todoList.Description
            // Map other properties as needed
        };

        _ = this.dbContext_.TodoLists.Add(newTodoListEntity);
        _ = this.dbContext_.SaveChanges();
    }

    void ITodoListService.UpdateTodoList(TodoList todoList)
    {
        var existingTodoList = this.dbContext_.TodoLists.Find(todoList.Id);
        if (existingTodoList == null)
        {
            throw new ArgumentException($"Todo list with ID {todoList.Id} not found");
        }

        existingTodoList.Title = todoList.Title;
        existingTodoList.Description = todoList.Description;

        _ = this.dbContext_.SaveChanges();
    }

    void ITodoListService.DeleteTodoList(int todoListId)
    {
        var todoListEntity = this.dbContext_.TodoLists.Find(todoListId);

        if (todoListEntity == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }

        _ = this.dbContext_.TodoLists.Remove(todoListEntity);
        _ = this.dbContext_.SaveChanges();
    }

    public IEnumerable<TodoList> GetTodoLists()
    {
        var todoListEntities = this.dbContext_.TodoLists.Include(t => t.Tasks).ToList();
        var todoLists = todoListEntities.Select(entity => new TodoList
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Tasks = entity.Tasks.Select(task => new Task
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate,
                Tags = task.Tags,
                Comments = task.Comments,
                DueDate = task.DueDate,
                Status = (Services.TaskStatus)task.Status,
            }
                ).ToList()
        }).ToList();

        return todoLists;
    }
}
