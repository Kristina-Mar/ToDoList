namespace ToDoList.Test;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentAssertions;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using NSubstitute.ExceptionExtensions;

public class DeleteUnitTests
{
    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.DeleteByIdAsync(Arg.Any<int>()).Returns(Task.CompletedTask);

        // Act
        var result = await controller.DeleteByIdAsync(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await repositoryMock.Received(1).DeleteByIdAsync(Arg.Any<int>());

        result.Should().BeOfType<NoContentResult>(); // FluentAssertions alternative
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.DeleteByIdAsync(Arg.Any<int>()).Throws(new KeyNotFoundException());

        // Act
        var result = await controller.DeleteByIdAsync(-1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repositoryMock.Received(1).DeleteByIdAsync(Arg.Any<int>());

        result.Should().BeOfType<NotFoundResult>(); // FluentAssertions alternative
    }

    [Fact]
    public async Task Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.When(r => r.DeleteByIdAsync(Arg.Any<int>())).Throws(new Exception());

        // Act
        var result = await controller.DeleteByIdAsync(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repositoryMock.Received(1).DeleteByIdAsync(Arg.Any<int>());

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500); // FluentAssertions alternative
    }
}
