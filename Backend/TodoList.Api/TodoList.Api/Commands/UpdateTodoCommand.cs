using MediatR;
using System;
using System.Text.Json.Serialization;
using TodoList.Api.Responses;

namespace TodoList.Api.Commands
{
    public class UpdateTodoCommand : IRequest<TodoResponse>
    {
        public UpdateTodoCommand(Guid id, bool isCompleted)
        {
            IsCompleted = isCompleted;
            Id = id;
        }

        public UpdateTodoCommand(Guid id, string description, bool isCompleted)
        {
            Description = description;
            IsCompleted = isCompleted;
            Id = id;
        }

        [JsonConstructor]
        public UpdateTodoCommand(Guid id, string description, bool isCompleted, DateTime? expireDate, int ownerId)
        {
            Description = description;
            IsCompleted = isCompleted;
            ExpireDate = expireDate;
            OwnerId = ownerId;
            Id = id;
        }

        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? ExpireDate { get; set; }

        public int OwnerId { get; set; }
    }
}