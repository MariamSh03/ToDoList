using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Services;
public class TodoList
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Task>? Tasks { get; set; }
}
