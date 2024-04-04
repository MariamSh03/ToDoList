using System.Collections.Generic;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TodoListService : ITodoListService
    {
        public void AddTodoList(TodoList todoList)
        {
            throw new NotImplementedException();
        }

        public void DeleteTodoList(int todoListId)
        {
            throw new NotImplementedException();
        }

        public TodoList GetTodoListById(int todoListId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<TodoList> GetTodoLists()
        {
            return null;
        }

        public void UpdateTodoList(int todoListId, TodoList updatedTodoList)
        {
            throw new NotImplementedException();
        }

        public void UpdateTodoList(TodoList todoList)
        {
            throw new NotImplementedException();
        }
    }
}
