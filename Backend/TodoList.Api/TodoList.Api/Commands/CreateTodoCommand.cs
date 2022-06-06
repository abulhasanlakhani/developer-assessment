using MediatR;
using System;
using TodoList.Api.Responses;

namespace TodoList.Api.Commands
{
    public class CreateTodoCommand : IRequest<TodoResponse>
    {
        public CreateTodoCommand(string description, bool isCompleted, DateTime? expireDate, int ownerId)
        {
            Description = description;
            IsCompleted = isCompleted;
            ExpireDate = expireDate;
            OwnerId = ownerId;
        }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? ExpireDate { get; set; }

        public int OwnerId { get; set; }
    }
}