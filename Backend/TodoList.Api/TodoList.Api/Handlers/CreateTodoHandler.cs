using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Commands;
using TodoList.Api.Responses;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;

namespace TodoList.Api.Handlers
{
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, TodoResponse>
    {
        private readonly ITodoRepository _todoRepository;

        public CreateTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoResponse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var newTodo = new TodoItem(Guid.NewGuid(), request.Description, request.IsCompleted, request.ExpireDate, request.OwnerId); 

            await _todoRepository.AddTodo(newTodo);

            return new TodoResponse(newTodo.Id, newTodo.Description, newTodo.IsCompleted, newTodo.ExpireDate, newTodo.OwnerId);
        }
    }
}
