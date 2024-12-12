namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context;

    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(ToDoItem item)
    {
        await context.ToDoItems.AddAsync(item);
        context.SaveChanges();
    }

    public async Task<IEnumerable<ToDoItem>> ReadAsync() => await context.ToDoItems.ToListAsync();

    public async Task<ToDoItem> ReadByIdAsync(int toDoItemId) => await context.ToDoItems.FindAsync(toDoItemId);

    public async Task UpdateByIdAsync(ToDoItem updatedItem)
    {
        var itemToUpdateInDb = await context.ToDoItems.FindAsync(updatedItem.ToDoItemId) ?? throw new KeyNotFoundException();
        context.Entry(itemToUpdateInDb).CurrentValues.SetValues(updatedItem);
        await context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int toDoItemId)
    {
        var itemToDeleteInDb = await context.ToDoItems.FindAsync(toDoItemId) ?? throw new KeyNotFoundException();
        context.ToDoItems.Remove(itemToDeleteInDb);
        await context.SaveChangesAsync();
    }
}
