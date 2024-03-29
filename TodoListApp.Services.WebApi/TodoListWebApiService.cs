using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TodoListWebApiService : ITodoListService
    {
        private readonly HttpClient _httpClient;

        public TodoListWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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
            throw new NotImplementedException();
        }

        public async Task<TodoListModel[]> GetTodoListsAsync()
        {
            // Make a GET request to the API endpoint to retrieve the list of to-do lists
            var response = await _httpClient.GetAsync("api/todolist");

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Deserialize the JSON response to TodoListModel array
            return await response.Content.ReadFromJsonAsync<TodoListModel[]>();
        }

        public void UpdateTodoList(TodoList todoList)
        {
            throw new NotImplementedException();
        }
    }
}
