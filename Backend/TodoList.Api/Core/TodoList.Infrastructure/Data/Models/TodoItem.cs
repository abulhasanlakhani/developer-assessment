namespace TodoList.Infrastructure.Data.Models;

public class TodoItem
{
    public TodoItem(Guid id, string description, bool isCompleted)
    {
        Id = id;
        Description = description;
        IsCompleted = isCompleted;
    }

    public Guid Id { get; set; }

    public string Description { get; set; }

    public bool IsCompleted { get; set; }
}