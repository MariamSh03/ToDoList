namespace TodoListApp.Services;
public interface ITodoListService
{
    IEnumerable<TodoList> GetTodoLists();
    TodoList GetTodoListById(int todoListId);
    void AddTodoList(TodoList todoList);
    void UpdateTodoList(TodoList todoList);
    void DeleteTodoList(int todoListId);
}
