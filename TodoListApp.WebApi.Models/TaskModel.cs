namespace TodoListApp.WebApi.Models;


public class TaskModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public string Assignee { get; set; }
    public string Tags { get; set; }
    public string Comments { get; set; }
}


public enum TaskStatus
{
    NotStarted,
    InProgress,
    Completed
}
