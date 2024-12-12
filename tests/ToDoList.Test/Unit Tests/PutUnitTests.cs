namespace ToDoList.Test;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

public class PutUnitTests
{
    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var updatedItem = new ToDoItemUpdateRequestDto("Updated name", "Updated category", "Updated description", true);

        repositoryMock.UpdateByIdAsync(Arg.Any<ToDoItem>()).Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateByIdAsync(1, updatedItem);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await repositoryMock.Received(1).UpdateByIdAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var updatedItem = new ToDoItemUpdateRequestDto("Updated name", "Updated category", "Updated description", true);

        repositoryMock.UpdateByIdAsync(Arg.Any<ToDoItem>()).Throws(new KeyNotFoundException());

        // Act
        var result = await controller.UpdateByIdAsync(2, updatedItem);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repositoryMock.Received(1).UpdateByIdAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var updatedItem = new ToDoItemUpdateRequestDto("Updated name", "Updated category", "Updated description", true);

        repositoryMock.When(r => r.UpdateByIdAsync(Arg.Any<ToDoItem>())).Do(r => throw new Exception());

        // Act
        var result = await controller.UpdateByIdAsync(1, updatedItem);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repositoryMock.Received(1).UpdateByIdAsync(Arg.Any<ToDoItem>());
    }
}
