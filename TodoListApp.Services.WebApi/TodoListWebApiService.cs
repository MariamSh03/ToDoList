using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


namespace TodoListApp.Services.WebApi
{
    public class TodoListWebApiService : ITodoListWebApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7093";

        public TodoListWebApiService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(BaseUrl);
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<TodoList>> GetTodoLists()
        {
            var response = await this._httpClient.GetAsync(BaseUrl + "/TodoList");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get todo lists. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<TodoList>>(content);
        }

        public async System.Threading.Tasks.Task AddTodoList(TodoList todoList)
        {
            var json = JsonConvert.SerializeObject(todoList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/TodoList", content);
            response.EnsureSuccessStatusCode();
        }

        public async System.Threading.Tasks.Task DeleteTodoList(int todoListId)
        {
            var response = await _httpClient.DeleteAsync($"/TodoList/{todoListId}");
            response.EnsureSuccessStatusCode();
        }

        public async System.Threading.Tasks.Task UpdateTodoList(TodoList todoList)
        {
            var json = JsonConvert.SerializeObject(todoList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/TodoList/{todoList.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TodoList> GetTodoListById(int todoListId)
        {
            var response = await _httpClient.GetAsync($"/TodoList/{todoListId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TodoList>(content);
        }
    }
}
