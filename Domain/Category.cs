using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<TodoTask>? Tasks { get; set; }
}