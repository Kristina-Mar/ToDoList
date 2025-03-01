namespace ToDoList.Frontend.Views;
using System.ComponentModel.DataAnnotations;

public class ToDoItemView
{
    public int ToDoItemId { get; set; }
    [Required(ErrorMessage = "Name is mandatory.")]
    [Length(1, 50)]
    public required string Name { get; set; }
    [StringLength(50)]
    public string? Category { get; set; }
    [Required(ErrorMessage = "Description is mandatory.")]
    [StringLength(250)]
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
    [Required(ErrorMessage = "Deadline is mandatory.")]
    public required DateTime Deadline { get; set; }
}
