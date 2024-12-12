namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Views;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();
    public Task<ToDoItemView?> ReadByIdAsync(int itemId);
    public Task CreateAsync(ToDoItemView toDoItemView);
    public Task UpdateByIdAsync(ToDoItemView toDoItemView);
    public Task DeleteByIdAsync(ToDoItemView toDoItemView);
}
