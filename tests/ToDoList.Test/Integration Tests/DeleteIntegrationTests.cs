namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using FluentAssertions;

public class DeleteIntegrationTests
{
    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContentAndDeletesItem()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../../../data/localdb.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem()
        {
            Name = "Delete test name",
            Category = "Delete test category",
            Description = "Delete test description",
            IsCompleted = false
        };

        context.Add(toDoItem);
        context.SaveChanges();

        // Act
        var result = await controller.DeleteByIdAsync(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(await repository.ReadByIdAsync(toDoItem.ToDoItemId));

        // FluentAssertions alternatives
        var nonExistentItem = await repository.ReadByIdAsync(toDoItem.ToDoItemId);
        nonExistentItem.Should().BeNull();
        var items = await repository.ReadAsync();
        items.Should().NotContain(i => i.ToDoItemId == toDoItem.ToDoItemId);
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../../../data/localdb.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        var invalidId = -1;

        // Act
        var result = await controller.DeleteByIdAsync(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.Null(await repository.ReadByIdAsync(invalidId));
    }
}
