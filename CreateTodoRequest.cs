namespace SampleCrud;

public record CreateTodoRequest(
    string Title,
    string? Description,
    bool? IsCompleted,
    DateTime? DueDate
);
