using MediatR;
using System.Collections.Generic;
using TodoList.Api.Responses;

namespace TodoList.Api.Queries
{
    public class GetAllTodosQuery : IRequest<List<TodoResponse>>
    {
    }
}
