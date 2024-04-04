namespace TodoListApp.Services;
public interface ITodoListService
{
    public IEnumerable<TodoList> GetTodoLists();
    public TodoList GetTodoListById(int todoListId);
    public void AddTodoList(TodoList todoList);
    public void UpdateTodoList(TodoList todoList);
    public void DeleteTodoList(int todoListId);
}
