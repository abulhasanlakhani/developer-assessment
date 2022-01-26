using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TodoList.Api.Controllers;
using TodoList.Infrastructure.Data.Models;
using TodoList.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace TodoList.Api.UnitTests.Api
{
    public class TodoControllerTests
    {
        [Fact]
        public async void GetTodo_ReturnsNotFoundResult_WhenTodoIsNotFound()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var todoId = Guid.NewGuid();
            todoRepository.GetTodoById(todoId).Returns(Task.FromResult<TodoItem>(null));

            // Act
            var result = await todoController.GetTodoItem(todoId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void GetTodo_ReturnsOkResult_WhenTodoIsFound()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var todo = new TodoItem(Guid.NewGuid(), "test todo", false);

            todoRepository.GetTodoById(todo.Id).Returns(Task.FromResult(todo));

            // Act
            var result = await todoController.GetTodoItem(todo.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetAllTodos_ReturnsOkResult_WithAllTodoRecords()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var todoList = new List<TodoItem>
            {
                new(Guid.NewGuid(), "test todo 1", false),
                new(Guid.NewGuid(), "test todo 2", false),
                new(Guid.NewGuid(), "test todo 3", false),
                new(Guid.NewGuid(), "test todo 4", false),
                new(Guid.NewGuid(), "test todo 5", false)
            };

            todoRepository.GetTodos().Returns(Task.FromResult(todoList));

            // Act
            var result = await todoController.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TodoItem>>(okResult.Value);

            Assert.True(returnValue.Count == todoList.Count);
        }

        [Fact]
        public async void Create_ReturnsNewlyCreatedTodoItem()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var newTodo = new TodoItem(Guid.NewGuid(), "test todo 1", false);

            todoRepository.AddTodo(newTodo).Returns(Task.CompletedTask);

            // Act
            var result = await todoController.PostTodoItem(newTodo);

            // Assert

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            
            Assert.Equal(returnValue.Description, newTodo.Description);
        }

        [Fact]
        public async void Create_ReturnsBadRequestIfDescriptionIsEmpty()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var newTodo = new TodoItem(Guid.NewGuid(), "", false);

            todoRepository.AddTodo(newTodo).Returns(Task.CompletedTask);

            // Act
            var result = await todoController.PostTodoItem(newTodo);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.True(returnValue == "Description is required");
        }

        [Fact]
        public async void Create_ReturnsBadRequestIfTodoItemIsNull()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var newTodo = new TodoItem(Guid.NewGuid(), "", false);

            todoRepository.AddTodo(newTodo).Returns(Task.CompletedTask);

            // Act
            var result = await todoController.PostTodoItem(null);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.True(returnValue == "Description is required");
        }

        [Fact]
        public async void Create_ReturnsBadRequestIfDescriptionAlreadyExists()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);

            var newTodo = new TodoItem(Guid.NewGuid(), "Item already exists", false);

            todoRepository.AddTodo(newTodo).Returns(Task.CompletedTask);
            todoRepository.TodoItemDescriptionExists(Arg.Any<string>()).Returns(true);

            // Act
            var result = await todoController.PostTodoItem(newTodo);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.True(returnValue == "Description already exists");
        }

        [Fact]
        public async void Edit_ReturnsBadRequestIfIdDoesntMatchWithTodoId()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);
            
            var newTodo = new TodoItem(Guid.NewGuid(), "", false);

            todoRepository.EditTodo(Guid.NewGuid(), newTodo).Returns(true);

            // Act
            var result = await todoController.PutTodoItem(Guid.NewGuid(), newTodo);

            // Assert

            var badRequestObjectResult = Assert.IsType<BadRequestResult>(result);
            Assert.True(badRequestObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async void Edit_ReturnsTrueIfIdMatchesWithTodoId()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);
            var todoId = Guid.NewGuid();
            var newTodo = new TodoItem(todoId, "", false);

            todoRepository.EditTodo(todoId, newTodo).Returns(true);

            // Act
            var result = await todoController.PutTodoItem(todoId, newTodo);

            // Assert

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.True(noContentResult.StatusCode == StatusCodes.Status204NoContent);
        }

        [Fact]
        public async void Edit_ReturnsFalseIfIdNotFound()
        {
            // Arrange
            var todoRepository = Substitute.For<ITodoRepository>();
            var todoController = new TodoItemsController(todoRepository);
            var todoId = Guid.NewGuid();
            var newTodo = new TodoItem(todoId, "", false);

            // Here it will return false acting as the id of the todoitem couldn't be found
            todoRepository.EditTodo(todoId, newTodo).Returns(false);

            // Act
            var result = await todoController.PutTodoItem(todoId, newTodo);

            // Assert

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.True(notFoundResult.StatusCode == StatusCodes.Status404NotFound);
        }
    }
}
