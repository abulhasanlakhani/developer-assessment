namespace TodoList.Infrastructure.Data.Models;

public class TodoItem
{
    public TodoItem(Guid id, string description, bool isCompleted, DateTime? expireDate, int ownerId)
    {
        Id = id;
        Description = description;
        IsCompleted = isCompleted;
        ExpireDate = expireDate;
        OwnerId = ownerId;
    }

    public Guid Id { get; set; }

    public string Description { get; set; }

    public DateTime? ExpireDate { get; set; }

    public bool IsCompleted { get; set; }

    public int OwnerId { get; set; }
}