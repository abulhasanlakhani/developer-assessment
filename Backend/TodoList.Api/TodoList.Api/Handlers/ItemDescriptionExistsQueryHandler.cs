using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Api.Queries;
using TodoList.Infrastructure.Repositories.Interfaces;

namespace TodoList.Api.Handlers
{
    public class ItemDescriptionExistsQueryHandler : IRequestHandler<ItemDescriptionExistsQuery, bool>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<ItemDescriptionExistsQueryHandler> _logger;

        public ItemDescriptionExistsQueryHandler(ITodoRepository todoRepository, ILogger<ItemDescriptionExistsQueryHandler> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }

        public Task<bool> Handle(ItemDescriptionExistsQuery request, CancellationToken cancellationToken)
        {
            var isDescriptionFound = _todoRepository.TodoItemDescriptionExists(request.TodoItemDescription);

            if (isDescriptionFound)
            {
                _logger.LogError("Todo with {description} already exists in the database", request.TodoItemDescription);
            }

            return Task.FromResult(isDescriptionFound);
        }
    }
}
