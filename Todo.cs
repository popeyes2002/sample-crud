namespace SampleCrud.Models;

public sealed record Todo
{
    public required int Id { get; init; }

    public required string Title { get; init; } = default!;

    public string? Description { get; init; }

    public required bool IsCompleted { get; init; }

    public DateTime? DueDate { get; init; }

    public required DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}
