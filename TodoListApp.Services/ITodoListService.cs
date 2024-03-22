namespace TodoListApp.Services;
public interface ITodoListService
{
    public IEnumerable<TodoList> GetTodoLists();
    public void AddTodoList(TodoList todoList);
    public void DeleteTodoList(int todoListId);
    public void UpdateTodoList(int todoListId, TodoList updatedTodoList);

}
