using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

public class TaskWebApiService : ITaskWebApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:7093";

    public TaskWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IEnumerable<TaskModel>> GetTasks()
    {
        var response = await _httpClient.GetAsync("/TodoLists/tasks");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<TaskModel>>(content);
    }

    public async Task<TaskModel> GetTaskById(int taskId)
    {
        var response = await _httpClient.GetAsync($"/TodoLists/tasks/{taskId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TaskModel>(content);
    }

    public async System.Threading.Tasks.Task AddTask(TaskModel task, int listId)
    {
        var json = JsonConvert.SerializeObject(task);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this._httpClient.PostAsync($"/TodoLists/{listId}/tasks", data);
        var responseContent = await response.Content.ReadAsStringAsync();
    }

    public async System.Threading.Tasks.Task UpdateTask(TaskModel task)
    {
        var json = JsonConvert.SerializeObject(task);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/TodoLists/tasks/{task.Id}", data);
        response.EnsureSuccessStatusCode();
    }

    public async System.Threading.Tasks.Task DeleteTask(int taskId)
    {
        var response = await _httpClient.DeleteAsync($"/TodoLists/tasks/{taskId}");
        response.EnsureSuccessStatusCode();
    }

    public async System.Threading.Tasks.Task UpdateTaskStatus(int taskId, string status)
    {
        var json = JsonConvert.SerializeObject(status);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/TodoLists/tasks/{taskId}/status", data);
        response.EnsureSuccessStatusCode();
    }

}
