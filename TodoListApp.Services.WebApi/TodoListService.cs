using System.Collections.Generic;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListService _todoListWebApiService;

        public TodoListService(ITodoListService todoListWebApiService)
        {
            _todoListWebApiService = todoListWebApiService;
        }

        public IEnumerable<TodoList> GetTodoLists()
        {
            return _todoListWebApiService.GetTodoLists();
        }

        public void AddTodoList(TodoList todoList)
        {
            _todoListWebApiService.AddTodoList(todoList);
        }

        public void DeleteTodoList(int todoListId)
        {
            _todoListWebApiService.DeleteTodoList(todoListId);
        }

        public void UpdateTodoList(TodoList todoList)
        {
            _todoListWebApiService.UpdateTodoList(todoList);
        }

        public TodoList GetTodoListById(int todoListId)
        {
            return _todoListWebApiService.GetTodoListById(todoListId);
        }
    }
}
