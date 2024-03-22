using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;

namespace TodoListApp.Services.Database;
public class TodoListDatabaseService : ITodoListService
{
    private readonly TodoListDbContext dbContext_;

    public TodoListDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext_ = dbContext;
    }

    public IEnumerable<TodoList> GetTodoLists()
    {
        // Retrieve the list of todo lists from the database
        var todoListEntities = this.dbContext_.TodoLists.ToList();
        var todoLists = todoListEntities.Select(entity => new TodoList
        {
            Id = entity.Id,
            Title = entity.Title,
            IsCompleted = entity.IsCompleted
            // Map other properties as needed
        }).ToList();

        return todoLists;
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
            IsCompleted = todoList.IsCompleted
            // Map other properties as needed
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
        existingTodoList.IsCompleted = updatedTodoList.IsCompleted;
        // Update other properties as needed

        _ = this.dbContext_.SaveChanges();
    }
}
