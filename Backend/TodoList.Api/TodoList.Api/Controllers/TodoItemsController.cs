using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TodoList.Infrastructure.Data.Models;
using MediatR;
using TodoList.Api.Queries;
using TodoList.Api.Commands;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoItem>))]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var query = new GetAllTodosQuery();
            var results = await _mediator.Send(query);
            return Ok(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItem>> GetTodoItem(Guid id)
        {
            var query = new GetTodoQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound($"Todo with {id} could not be found in the database");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTodoItem(Guid id, UpdateTodoCommand updateTodoCommand)
        {
            if (id != updateTodoCommand.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(updateTodoCommand);
            
            // If modification failed then we will get false as a result
            return result != null ? NoContent() : NotFound();
        } 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTodoItem(CreateTodoCommand createTodoCommand)
        {
            if (string.IsNullOrEmpty(createTodoCommand?.Description))
            {
                return BadRequest("Description is required");
            }

            var itemExistsQuery = new ItemDescriptionExistsQuery(createTodoCommand.Description);
            var itemExistsQueryResponse = await _mediator.Send(itemExistsQuery);

            if (itemExistsQueryResponse)
            {
                return BadRequest("Description already exists");
            }

            var result = await _mediator.Send(createTodoCommand);
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = result.Id }, result);
        }
    }
}
