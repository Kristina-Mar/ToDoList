namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemUpdateRequestDto(string Name, string? Category, string Description, bool IsCompleted)
{
    public ToDoItem ToDomain() => new() { Name = Name, Category = Category, Description = Description, IsCompleted = IsCompleted };
}
