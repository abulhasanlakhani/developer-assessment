using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoRepository todoRepository, TodoContext context, ILogger<TodoItemsController> logger)
        {
            _todoRepository = todoRepository;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoItem>))]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var results = await _todoRepository.GetTodos();
            return Ok(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItem>> GetTodoItem(Guid id)
        {
            var result = await _todoRepository.GetTodoById(id);

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
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var result = await _todoRepository.EditTodo(id, todoItem);
            
            // If modification failed then we will get false as a result
            return result ? NoContent() : NotFound();
        } 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return BadRequest("Description is required");
            }

            if (TodoItemDescriptionExists(todoItem.Description))
            {
                return BadRequest("Description already exists");
            }

            await _todoRepository.AddTodo(todoItem);
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        } 
        
        private bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
