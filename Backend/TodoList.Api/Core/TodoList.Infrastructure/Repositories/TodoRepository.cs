using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;

namespace TodoList.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;
    private readonly ILogger<TodoRepository> _logger;

    public TodoRepository(TodoContext context, ILogger<TodoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TodoItem>> GetTodos()
    {
        var todos = await _context.TodoItems.ToListAsync();

        _logger?.LogInformation($"{todos.Count} todos retrieved from the database");

        return todos;
    }

    public async Task<TodoItem?> GetTodoById(Guid id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo == null)
        {
            _logger?.LogError("todo with ID: {id} could not be found in the database", id);
            return null;
        }

        _logger?.LogInformation($"{id} todos retrieved from the database");

        return todo;
    }

    public async Task<bool> EditTodo(Guid id, TodoItem todoItemToEdit)
    {
        _context.Entry(todoItemToEdit).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemIdExists(id))
            {
                _logger?.LogError($"todo with ID: {id} could not be found in the database");
                return false;
            }

            throw;
        }

        _logger?.LogInformation($"Todo with ID {todoItemToEdit.Id} has been updated in the database!");
        return true;
    }

    public async Task AddTodo(TodoItem newTodoItem)
    {
        if (string.IsNullOrEmpty(newTodoItem.Description))
        {
            throw new ArgumentNullException(nameof(newTodoItem));
        }

        newTodoItem.Id = Guid.NewGuid();
        newTodoItem.IsCompleted = false;

        _context.TodoItems.Add(newTodoItem);
        await _context.SaveChangesAsync();

        _logger?.LogInformation($"Todo with ID {newTodoItem.Id} has been added to the database!");
    }

    public bool TodoItemDescriptionExists(string description)
    {
        return _context.TodoItems
            .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant());
    }

    private bool TodoItemIdExists(Guid id)
    {
        return _context.TodoItems.Any(x => x.Id == id);
    }

    
}