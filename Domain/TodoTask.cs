using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TodoTask
{
    [Key]
    public Guid TaskId { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    [MaxLength(64)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public EPriority Priority { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? Deadline { get; set; }

    [Display(Name = "Completed")]
    public bool IsCompleted { get; set; }
}