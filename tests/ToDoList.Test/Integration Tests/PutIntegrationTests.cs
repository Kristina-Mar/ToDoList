namespace ToDoList.Test;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

public class PutIntegrationTests
{
    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContentAndUpdatesItem()
    {
        // Arrange
        var dbPath = "Data Source=../../../../../data/localdb.db";
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", dbPath);
        var context = new ToDoItemsContext();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem()
        {
            Name = "Put test name",
            Description = "Put test description",
            IsCompleted = false,
            Deadline = new DateTime(2025, 3, 1)
        };
        context.Add(toDoItem);
        context.SaveChanges();

        var updatedItem = new ToDoItemUpdateRequestDto("Updated name", "Updated category", "Updated description", true, new DateTime(2025, 2, 1));

        // Act
        var result = await controller.UpdateByIdAsync(toDoItem.ToDoItemId, updatedItem);
        var updatedItemInList = await repository.ReadByIdAsync(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(updatedItem.Name, updatedItemInList.Name);
        Assert.Equal(updatedItem.Category, updatedItemInList.Category);
        Assert.Equal(updatedItem.Description, updatedItemInList.Description);
        Assert.Equal(updatedItem.IsCompleted, updatedItemInList.IsCompleted);
        Assert.Equal(updatedItem.Deadline, updatedItemInList.Deadline);

        // FluentAssertions alternative
        updatedItemInList.Name.Should().NotBe("Put test name");
        updatedItemInList.Description.Should().Be(updatedItem.Description);
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var dbPath = "Data Source=../../../../../data/localdb.db";
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", dbPath);
        var context = new ToDoItemsContext();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        int invalidId = -1;
        var updatedItem = new ToDoItemUpdateRequestDto("Updated name", "Updated category", "Updated description", true, new DateTime(2025, 2, 1));

        // Act
        var result = await controller.UpdateByIdAsync(invalidId, updatedItem);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.Null(await repository.ReadByIdAsync(invalidId));
    }
}
