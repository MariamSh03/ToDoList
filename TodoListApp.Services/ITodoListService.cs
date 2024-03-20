namespace TodoListApp.Services;
public interface ITodoListService
{
    public IEnumerable<TodoList> GetTodoLists();
}
