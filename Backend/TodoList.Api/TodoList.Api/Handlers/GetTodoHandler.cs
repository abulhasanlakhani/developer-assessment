using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Api.Queries;
using TodoList.Api.Responses;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;

namespace TodoList.Api.Handlers
{
    public class GetTodoHandler : IRequestHandler<GetTodoQuery, TodoResponse>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<GetTodoHandler> _logger;

        public GetTodoHandler(ITodoRepository todoRepository, ILogger<GetTodoHandler> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }

        public async Task<TodoResponse> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var todoItem = await _todoRepository.GetTodoById(request.Id);

            if (todoItem == null)
            {
                _logger.LogError("Todo with {id} could not be found in the database", request.Id);
                return null;
            }

            var response = MapTodoDtoToTodoResponse(todoItem);

            return response;
        }

        private static TodoResponse MapTodoDtoToTodoResponse(TodoItem todo)
        {
            TodoResponse newTodo = new(todo.Id, todo.Description, todo.IsCompleted, todo.ExpireDate, todo.OwnerId);

            return newTodo;
        }
    }
}
