namespace ToDoList.Test;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

public class GetUnitTests
{
    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var toDoItem1Dto = new ToDoItemGetResponseDto
        {
            ToDoItemId = 1,
            Name = "Test name",
            Category = "Work",
            Description = "Test description",
            IsCompleted = false
        };
        var toDoItem2Dto = new ToDoItemGetResponseDto
        {
            ToDoItemId = 2,
            Name = "Test name",
            Category = null,
            Description = "Test description",
            IsCompleted = true
        };

        List<ToDoItemGetResponseDto> allItemsExpected = [toDoItem1Dto, toDoItem2Dto];

        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test name",
            Category = "Work",
            Description = "Test description",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Test name",
            Category = null,
            Description = "Test description",
            IsCompleted = true
        };

        List<ToDoItem> allItemsFromRepository = [toDoItem1, toDoItem2];

        repositoryMock.ReadAsync().Returns(allItemsFromRepository);

        // Act
        var result = await controller.ReadAsync();

        // Assert
        var resultResult = Assert.IsType<OkObjectResult>(result.Result);

        var value = resultResult.Value as List<ToDoItemGetResponseDto>;

        Assert.NotNull(value);
        Assert.Equal(allItemsExpected, resultResult.Value);
        Assert.Equal(2, value.Count);
        await repositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadAsync().ReturnsNull();

        // Act
        var result = await controller.ReadAsync();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        await repositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadAsync().Throws(new Exception());

        // Act
        var result = await controller.ReadAsync();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test name",
            Category = "Test category",
            Description = "Test description",
            IsCompleted = false
        };

        var expectedReturnedItem = new ToDoItemGetResponseDto
        {
            ToDoItemId = 1,
            Name = "Test name",
            Category = "Test category",
            Description = "Test description",
            IsCompleted = false
        };

        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(toDoItem);

        // Act
        var result = await controller.ReadByIdAsync(1);

        // Assert
        var resultResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = resultResult.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);
        Assert.Equal(expectedReturnedItem, value);
        await repositoryMock.Received(1).ReadByIdAsync(Arg.Any<int>());
    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadByIdAsync(Arg.Any<int>()).ReturnsNull();

        // Act
        var result = await controller.ReadByIdAsync(2);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        await repositoryMock.Received(1).ReadByIdAsync(Arg.Any<int>());
    }

    [Fact]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        //repositoryMock.When(r => r.ReadById(Arg.Any<int>())).Do(r => throw new Exception()); alternative syntax
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Throws(new Exception());

        // Act
        var result = await controller.ReadByIdAsync(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repositoryMock.Received(1).ReadByIdAsync(Arg.Any<int>());
    }
}
