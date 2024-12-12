namespace ToDoList.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController(IRepositoryAsync<ToDoItem> repositoryAsync) : ControllerBase
{
    private readonly IRepositoryAsync<ToDoItem> repositoryAsync = repositoryAsync;

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();

        try
        {
            await repositoryAsync.CreateAsync(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(ReadByIdAsync), new { toDoItemId = item.ToDoItemId }, ToDoItemGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> ReadAsync()
    {
        IEnumerable<ToDoItem> allItems = [];
        try
        {
            allItems = await repositoryAsync.ReadAsync();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return (allItems != null) ? Ok(allItems.Select(ToDoItemGetResponseDto.FromDomain).ToList()) : NotFound();
    }

    [HttpGet("{toDoItemId:int}")]
    [ActionName(nameof(ReadByIdAsync))]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadByIdAsync(int toDoItemId)
    {
        ToDoItem? requestedItem;
        try
        {
            requestedItem = await repositoryAsync.ReadByIdAsync(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return (requestedItem != null) ? Ok(ToDoItemGetResponseDto.FromDomain(requestedItem)) : NotFound();
    }

    [HttpPut("{toDoItemId:int}")]
    public async Task<IActionResult> UpdateByIdAsync(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        var updatedItem = request.ToDomain();
        updatedItem.ToDoItemId = toDoItemId;

        try
        {
            await repositoryAsync.UpdateByIdAsync(updatedItem);
        }
        catch (Exception ex)
        {
            if (ex is KeyNotFoundException)
            {
                return NotFound();
            }
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<ActionResult> DeleteByIdAsync(int toDoItemId)
    {
        try
        {
            await repositoryAsync.DeleteByIdAsync(toDoItemId);
        }
        catch (Exception ex)
        {
            if (ex is KeyNotFoundException)
            {
                return NotFound();
            }
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}
