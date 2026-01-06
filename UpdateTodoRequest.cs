namespace SampleCrud;

public record UpdateTodoRequest(
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime? DueDate
);
