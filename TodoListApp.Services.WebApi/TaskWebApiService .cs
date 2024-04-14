using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TaskWebApiService : ITaskWebApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7093";

        public TaskWebApiService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(BaseUrl);
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<TaskModel>> GetTasks()
        {
            var response = await this._httpClient.GetAsync(BaseUrl + "/Task");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get tasks. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<TaskModel>>(content);
        }

        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            var json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/Task", content);
            response.EnsureSuccessStatusCode();
        }

        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            var response = await _httpClient.DeleteAsync($"/Task/{taskId}");
            response.EnsureSuccessStatusCode();
        }

        public async System.Threading.Tasks.Task UpdateTask(TaskModel task)
        {
            var json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/Task/{task.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TaskModel> GetTaskById(int taskId)
        {
            var response = await _httpClient.GetAsync($"/Task/{taskId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TaskModel>(content);
        }
    }
}
