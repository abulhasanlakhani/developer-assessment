using System;
using MediatR;
using TodoList.Api.Responses;

namespace TodoList.Api.Queries
{
    public class GetTodoQuery : IRequest<TodoResponse>
    {
        public GetTodoQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
