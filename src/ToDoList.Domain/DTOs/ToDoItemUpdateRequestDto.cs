namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemUpdateRequestDto(string Name, string? Category, string Description, bool IsCompleted, DateTime Deadline)
{
    public ToDoItem ToDomain() => new() { Name = Name, Category = Category, Description = Description, IsCompleted = IsCompleted, Deadline = Deadline };
}
