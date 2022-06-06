using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Queries;
using TodoList.Api.Responses;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;

namespace TodoList.Api.Handlers
{
    public class GetAllTodosHandler : IRequestHandler<GetAllTodosQuery, List<TodoResponse>>
    {
        private readonly ITodoRepository _todoRepository;

        public GetAllTodosHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<List<TodoResponse>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            var todos = await _todoRepository.GetTodos();
            var todosResponse = MapTodosDtosToTodosResponse(todos);
            
            return todosResponse;
        }

        private static List<TodoResponse> MapTodosDtosToTodosResponse(IEnumerable<TodoItem> todos)
        {
            return todos.Select(todo => new TodoResponse(todo.Id, todo.Description, todo.IsCompleted, todo.ExpireDate, todo.OwnerId)).ToList();
        }
    }
}
