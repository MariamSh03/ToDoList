using System;
using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Services.Database
{
    public class TaskEntity
    {
        [Key]
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
}
