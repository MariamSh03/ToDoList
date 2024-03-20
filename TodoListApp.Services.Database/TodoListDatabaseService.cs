using TodoListApp.Services;

namespace TodoListApp.Services.Database;
public class TodoListDatabaseService : ITodoListService
{
    private readonly TodoListDbContext _dbContext;

    public TodoListDatabaseService(TodoListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<TodoList> GetTodoLists()
    {
        // Retrieve the list of todo lists from the database
        var todoListEntities = _dbContext.TodoLists.ToList();
        var todoLists = todoListEntities.Select(entity => new TodoList
        {
            Id = entity.Id,
            Title = entity.Title,
            IsCompleted = entity.IsCompleted
            // Map other properties as needed
        }).ToList();

        return todoLists;
    }
}
