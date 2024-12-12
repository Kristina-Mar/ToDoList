using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Views;
using ToDoList.Frontend.Clients;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    public async Task CreateAsync(ToDoItemView toDoItemView)
    {
        var itemToCreateDto = new ToDoItemCreateRequestDto(toDoItemView.Name, toDoItemView.Category, toDoItemView.Description, toDoItemView.IsCompleted);
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/ToDoItems", itemToCreateDto);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var newItem = await response.Content.ReadFromJsonAsync<ToDoItemGetResponseDto>();
                Console.WriteLine($"POST request successful: Created a ToDoItem with Id {newItem.ToDoItemId}.");
                return;
            }
            else
            {
                Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
        }

    }

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var ToDoItemsView = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

        ToDoItemsView = response.Select(dto => new ToDoItemView
        {
            ToDoItemId = dto.ToDoItemId,
            Name = dto.Name,
            Category = dto.Category,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        }).ToList();

        return ToDoItemsView;
    }

    public async Task<ToDoItemView?> ReadByIdAsync(int itemId)
    {
        try
        {
            var requestedItemDto = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");

            if (requestedItemDto is null)
            {
                Console.WriteLine($"GET request failed: Item with {itemId} id not found.");
                throw new ArgumentException($"Given id {itemId} does not exist.");
            }
            else
            {
                var requestedItemView = new ToDoItemView
                {
                    ToDoItemId = requestedItemDto.ToDoItemId,
                    Name = requestedItemDto.Name,
                    Category = requestedItemDto.Category,
                    Description = requestedItemDto.Description,
                    IsCompleted = requestedItemDto.IsCompleted
                };

                return requestedItemView;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
            return null;
        }

    }

    public async Task UpdateByIdAsync(ToDoItemView toDoItemView)
    {
        var toDoItemUpdateDto = new ToDoItemUpdateRequestDto(toDoItemView.Name, toDoItemView.Category, toDoItemView.Description, toDoItemView.IsCompleted);

        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{toDoItemView.ToDoItemId}", toDoItemUpdateDto);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"PUT request successful: Updated ToDoItem with id {toDoItemView.ToDoItemId}.");
                return;
            }
            else
            {
                Console.WriteLine($"PUT request failed: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
        }
    }

    public async Task DeleteByIdAsync(ToDoItemView toDoItemView)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/ToDoItems/{toDoItemView.ToDoItemId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"DELETE request successful: Deleted ToDoItem with id {toDoItemView.ToDoItemId}.");
                return;
            }
            else
            {
                Console.WriteLine($"DELETE request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
        }
    }
}
