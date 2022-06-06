using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TodoList.Api.Commands;
using TodoList.Api.Controllers;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace TodoList.Api.UnitTests.Api
{
    public class TodoControllerTests : IDisposable
    {
        private ITodoRepository _todoRepository;
        private IMediator _mediator;
        
        public TodoControllerTests()
        {
            // Arrange
            _todoRepository = Substitute.For<ITodoRepository>();
            _mediator = Substitute.For<IMediator>();
        }

        public void Dispose()
        {
            _todoRepository = null;
            _mediator = null;
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetTodo_ReturnsNotFoundResult_WhenTodoIsNotFound()
        {
            var todoController = new TodoItemsController(_mediator);

            var todoId = Guid.NewGuid();
            
            _todoRepository.GetTodoById(todoId)
                .Returns(Task.FromResult<TodoItem>(null));

            // Act
            var result = await todoController.GetTodoItem(todoId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTodo_ReturnsOkResult_WhenTodoIsFound()
        {
            var todoController = new TodoItemsController(_mediator);

            var todo = new TodoItem(Guid.NewGuid(), "test todo", false, null, -1);

            _todoRepository.GetTodoById(todo.Id)
                .Returns(Task.FromResult(todo));

            // Act
            var result = await todoController.GetTodoItem(todo.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAllTodos_ReturnsOkResult_WithAllTodoRecords()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);

            var todoList = new List<TodoItem>
            {
                new(Guid.NewGuid(), "test todo 1", false, null, -1),
                new(Guid.NewGuid(), "test todo 2", false, null, -1),
                new(Guid.NewGuid(), "test todo 3", false, null, -1),
                new(Guid.NewGuid(), "test todo 4", false, null, -1),
                new(Guid.NewGuid(), "test todo 5", false, null, -1)
            };

            _todoRepository.GetTodos()
                .Returns(Task.FromResult(todoList));

            // Act
            var result = await todoController.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TodoItem>>(okResult.Value);

            Assert.True(returnValue.Count == todoList.Count);
        }

        [Fact]
        public async Task Create_ReturnsNewlyCreatedTodoItem()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);

            var createTodoCommand = new CreateTodoCommand("Test todo 1", false, null, -1);
            var newTodo = new TodoItem(Guid.NewGuid(), createTodoCommand.Description, createTodoCommand.IsCompleted, createTodoCommand.ExpireDate, createTodoCommand.OwnerId);

            _todoRepository.AddTodo(newTodo)
                .Returns(Task.CompletedTask);

            // Act
            var result = await todoController.PostTodoItem(createTodoCommand);

            // Assert

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            
            Assert.Equal(returnValue.Description, newTodo.Description);
        }

        [Fact]
        public async Task Create_ReturnsBadRequestIfDescriptionIsEmpty()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);

            var createTodoCommand = new CreateTodoCommand("", false, null, -1);
            var newTodo = new TodoItem(Guid.NewGuid(), createTodoCommand.Description, createTodoCommand.IsCompleted, createTodoCommand.ExpireDate, createTodoCommand.OwnerId);

            _todoRepository.AddTodo(newTodo).Returns(Task.CompletedTask);

            // Act
            var result = await todoController.PostTodoItem(createTodoCommand);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.True(returnValue == "Description is required");
        }

        [Fact]
        public async Task Create_ReturnsBadRequestIfTodoItemIsNull()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);

            var newTodo = new TodoItem(Guid.NewGuid(), "", false, null, -1);

            _todoRepository.AddTodo(newTodo).Returns(Task.CompletedTask);

            // Act
            var result = await todoController.PostTodoItem(null);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.True(returnValue == "Description is required");
        }

        [Fact]
        public async Task Create_ReturnsBadRequestIfDescriptionAlreadyExists()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);

            var createTodoCommand = new CreateTodoCommand("Item already exists", false, null, -1);
            var newTodo = new TodoItem(Guid.NewGuid(), createTodoCommand.Description, createTodoCommand.IsCompleted, createTodoCommand.ExpireDate, createTodoCommand.OwnerId);
            
            _todoRepository.AddTodo(newTodo)
                .Returns(Task.CompletedTask);
            
            _todoRepository.TodoItemDescriptionExists(Arg.Any<string>()).Returns(true);

            // Act
            var result = await todoController.PostTodoItem(createTodoCommand);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.True(returnValue == "Description already exists");
        }

        [Fact]
        public async Task Edit_ReturnsBadRequestIfIdDoesntMatchWithTodoId()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);

            var updateTodoCommand = new UpdateTodoCommand(Guid.NewGuid(), "", false, null, -1);
            var newTodo = new TodoItem(updateTodoCommand.Id, updateTodoCommand.Description, updateTodoCommand.IsCompleted, updateTodoCommand.ExpireDate, updateTodoCommand.OwnerId);

            _todoRepository.EditTodo(Guid.NewGuid(), newTodo).Returns(true);

            // Act
            var result = await todoController.PutTodoItem(Guid.NewGuid(), updateTodoCommand);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestResult>(result);
            Assert.True(badRequestObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Edit_ReturnsTrueIfIdMatchesWithTodoId()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);
            var todoId = Guid.NewGuid();

            var updateTodoCommand = new UpdateTodoCommand(todoId, "", false, null, -1);
            var newTodo = new TodoItem(updateTodoCommand.Id, updateTodoCommand.Description, updateTodoCommand.IsCompleted, updateTodoCommand.ExpireDate, updateTodoCommand.OwnerId);

            _todoRepository.EditTodo(todoId, newTodo).Returns(true);

            // Act
            var result = await todoController.PutTodoItem(todoId, updateTodoCommand);

            // Assert

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.True(noContentResult.StatusCode == StatusCodes.Status204NoContent);
        }

        [Fact]
        public async void Edit_ReturnsFalseIfIdNotFound()
        {
            // Arrange
            var todoController = new TodoItemsController(_mediator);
            var todoId = Guid.NewGuid();

            var updateTodoCommand = new UpdateTodoCommand(todoId, "", false, null, -1);
            var newTodo = new TodoItem(updateTodoCommand.Id, updateTodoCommand.Description, updateTodoCommand.IsCompleted, updateTodoCommand.ExpireDate, updateTodoCommand.OwnerId);

            // Here it will return false acting as the id of the todoitem couldn't be found
            _todoRepository.EditTodo(todoId, newTodo).Returns(false);

            // Act
            var result = await todoController.PutTodoItem(todoId, updateTodoCommand);

            // Assert

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.True(notFoundResult.StatusCode == StatusCodes.Status404NotFound);
        }
    }
}
