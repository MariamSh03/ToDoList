using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Database;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoListDbContext _dbContext;

    public TodoController(TodoListDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
