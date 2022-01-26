using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Repositories.Interfaces;

public interface ITodoRepository
{
    public Task<List<TodoItem>> GetTodos();
    public Task<TodoItem?> GetTodoById(Guid id);
    public Task<bool> EditTodo(Guid id, TodoItem todoItemToEdit);
    public Task AddTodo(TodoItem newTodoItem);
}