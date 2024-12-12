namespace ToDoList.Persistence.Repositories;

public interface IRepositoryAsync<T> where T : class
{
    public Task CreateAsync(T item);
    public Task<T?> ReadByIdAsync(int toDoItemId);
    public Task<IEnumerable<T>> ReadAsync();
    public Task UpdateByIdAsync(T updatedItem);
    public Task DeleteByIdAsync(int toDoItemId);
}
