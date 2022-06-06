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
    public class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, TodoResponse>
    {
        private readonly ITodoRepository _todoRepository;

        public UpdateTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoResponse> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todoItemToUpdate = new TodoItem(request.Id, request.Description, request.IsCompleted, request.ExpireDate, request.OwnerId);

            var result = await _todoRepository.EditTodo(request.Id, todoItemToUpdate);

            return result 
                ? new TodoResponse(todoItemToUpdate.Id, todoItemToUpdate.Description, todoItemToUpdate.IsCompleted, todoItemToUpdate.ExpireDate, todoItemToUpdate.OwnerId)
                : null;
        }
    }
}
