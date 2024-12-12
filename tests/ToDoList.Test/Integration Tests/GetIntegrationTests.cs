namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

public class GetIntegrationTests
{
    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOkAndAllItems()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../../../data/localdb.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem()
        {
            Name = "Get test name",
            Category = "Get test category",
            Description = "Get test description",
            IsCompleted = false
        };
        context.Add(toDoItem);
        context.SaveChanges();

        // Act
        var result = await controller.ReadAsync();

        // Assert
        var resultResult = Assert.IsType<OkObjectResult>(result.Result);

        var value = resultResult.Value as IEnumerable<ToDoItemGetResponseDto>;
        Assert.NotNull(value);

        var newItem = value.First(i => i.ToDoItemId == toDoItem.ToDoItemId);
        Assert.NotNull(newItem);
        Assert.Equal(toDoItem.Name, newItem.Name);
        Assert.Equal(toDoItem.Category, newItem.Category);
        Assert.Equal(toDoItem.Description, newItem.Description);
        Assert.Equal(toDoItem.IsCompleted, newItem.IsCompleted);
    }

    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOkAndItem()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../../../data/localdb.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem()
        {
            Name = "Get test name",
            Category = "Get test category",
            Description = "Get test description",
            IsCompleted = false
        };
        context.Add(toDoItem);
        context.SaveChanges();

        // Act
        var result = await controller.ReadByIdAsync(toDoItem.ToDoItemId);

        // Assert
        var resultResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = resultResult.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);
        Assert.Equal(toDoItem.Name, value.Name);
        Assert.Equal(toDoItem.Category, value.Category);
        Assert.Equal(toDoItem.Description, value.Description);
        Assert.Equal(toDoItem.IsCompleted, value.IsCompleted);
    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../../../data/localdb.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        int invalidId = -1;

        // Act
        var result = await controller.ReadByIdAsync(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.Null(await repository.ReadByIdAsync(invalidId));
    }
}