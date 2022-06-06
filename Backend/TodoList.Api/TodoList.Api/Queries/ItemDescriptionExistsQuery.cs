using MediatR;

namespace TodoList.Api.Queries
{
    public class ItemDescriptionExistsQuery : IRequest<bool>
    {
        public ItemDescriptionExistsQuery(string todoItemDescription)
        {
            TodoItemDescription = todoItemDescription;
        }

        public string TodoItemDescription { get; set; }
    }
}
