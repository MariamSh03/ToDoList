using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Services;
public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoList>> GetTodoLists();
    Task<TodoList> GetTodoListById(int todoListId);
    System.Threading.Tasks.Task AddTodoList(TodoList todoList);
    System.Threading.Tasks.Task UpdateTodoList(TodoList todoList);
    System.Threading.Tasks.Task DeleteTodoList(int todoListId);
}
