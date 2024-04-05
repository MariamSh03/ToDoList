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
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate,
                DueDate = task.DueDate,
                Status = task.Status,
                Assignee = task.Assignee,
                Tags = task.Tags,
                Comments = task.Comments,
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

        existingTodoList.Title = updatedTodoList.Title;
        existingTodoList.Description = updatedTodoList.Description;
        existingTodoList.Tasks = updatedTodoList.Tasks.Select(task => new TaskEntity
        {
            Title = task.Title,
            Description = task.Description,
            CreationDate = task.CreationDate,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            Tags = task.Tags,
            Comments = task.Comments
        }).ToList();

        _ = this.dbContext_.SaveChanges();
    }

    public TodoList GetTodoListById(int todoListId)
    {
        var todoListEntity = this.dbContext_.TodoLists.Include(t => t.Tasks).ToList().FirstOrDefault(t => t.Id == todoListId);

        if (todoListEntity == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }


        var todoList = new TodoList
        {
            Id = todoListEntity.Id,
            Title = todoListEntity.Title,
            Description = todoListEntity.Description,
            Tasks = todoListEntity.Tasks.Select(task => new Task
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate,
                DueDate = task.DueDate,
                Status = task.Status,
                Assignee = task.Assignee,
                Tags = task.Tags,
                Comments = task.Comments
            }).ToList()
        };

        return todoList;
    }

    public void UpdateTodoList(TodoList todoList)
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
                Status = task.Status,
                Assignee = task.Assignee,
            }
                ).ToList()
        }).ToList();

        return todoLists;
    }

    public void DeleteTask(int todoListId, int taskId)
    {
        // Find the todo list by ID
        var todoListEntity = this.dbContext_.TodoLists
            .Include(t => t.Tasks)
            .FirstOrDefault(t => t.Id == todoListId);

        if (todoListEntity == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }

        // Find the task within the todo list
        var taskEntity = todoListEntity.Tasks.FirstOrDefault(task => task.Id == taskId);

        if (taskEntity == null)
        {
            throw new ArgumentException($"Task with ID {taskId} not found in todo list with ID {todoListId}");
        }

        // Remove the task from the todo list
        todoListEntity.Tasks.Remove(taskEntity);

        // Save changes to the database
        _ = this.dbContext_.SaveChanges();
    }

    public void AddTask(int todoListId, Task task)
    {
        // Find the todo list by ID
        var todoListEntity = this.dbContext_.TodoLists
            .Include(t => t.Tasks)
            .FirstOrDefault(t => t.Id == todoListId);

        if (todoListEntity == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }

        // Create a new task entity
        var newTaskEntity = new TaskEntity
        {
            Title = task.Title,
            Description = task.Description,
            CreationDate = task.CreationDate,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            Tags = task.Tags,
            Comments = task.Comments
        };

        // Add the task entity to the todo list
        todoListEntity.Tasks ??= new List<TaskEntity>(); // Ensure Tasks collection is initialized
        todoListEntity.Tasks.Add(newTaskEntity);

        // Save changes to the database
        _ = this.dbContext_.SaveChanges();
    }

    public void EditTask(int todoListId, Task task)
    {
        // Find the todo list by ID
        var todoListEntity = this.dbContext_.TodoLists
            .Include(t => t.Tasks)
            .FirstOrDefault(t => t.Id == todoListId);

        if (todoListEntity == null)
        {
            throw new ArgumentException($"Todo list with ID {todoListId} not found");
        }

        // Find the task within the todo list
        var taskEntity = todoListEntity.Tasks.FirstOrDefault(t => t.Id == task.Id);

        if (taskEntity == null)
        {
            throw new ArgumentException($"Task with ID {task.Id} not found in todo list with ID {todoListId}");
        }

        // Update the task properties
        taskEntity.Title = task.Title;
        taskEntity.Description = task.Description;
        taskEntity.CreationDate = task.CreationDate;
        taskEntity.DueDate = task.DueDate;
        taskEntity.Status = task.Status;
        taskEntity.Assignee = task.Assignee;
        taskEntity.Tags = task.Tags;
        taskEntity.Comments = task.Comments;

        // Save changes to the database
        _ = this.dbContext_.SaveChanges();
    }
}
