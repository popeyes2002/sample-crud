namespace SampleCrud;

using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using SampleCrud.Models;
using SampleCrud.Repositories;

public class TodoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos").WithTags("Todos");

        group.MapGet("/", GetAllTodos).WithName("GetAllTodos");

        group.MapGet("/{id:int}", GetTodoById).WithName("GetTodoById");

        group.MapPost("/", CreateTodo).WithName("CreateTodo");

        group.MapPut("/{id:int}", UpdateTodo).WithName("UpdateTodo");

        group.MapDelete("/{id:int}", DeleteTodo).WithName("DeleteTodo");
    }

    private static async Task<Ok<IEnumerable<Todo>>> GetAllTodos(ITodoRepository repository)
    {
        var todos = await repository.GetAllAsync();
        return TypedResults.Ok(todos);
    }

    private static async Task<Results<Ok<Todo>, NotFound>> GetTodoById(
        int id,
        ITodoRepository repository
    )
    {
        var todo = await repository.GetByIdAsync(id);
        return todo is not null ? TypedResults.Ok(todo) : TypedResults.NotFound();
    }

    private static async Task<Results<Created<Todo>, ProblemHttpResult>> CreateTodo(
        CreateTodoRequest request,
        ITodoRepository repository
    )
    {
        var todo = new Todo
        {
            Id = 0,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsCompleted ?? false,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        var id = await repository.CreateAsync(todo);
        var createdTodo = await repository.GetByIdAsync(id);

        return createdTodo is not null
            ? TypedResults.Created($"/api/todos/{id}", createdTodo)
            : TypedResults.Problem("Failed to create todo");
    }

    private static async Task<Results<NoContent, NotFound, ProblemHttpResult>> UpdateTodo(
        int id,
        UpdateTodoRequest request,
        ITodoRepository repository
    )
    {
        var existingTodo = await repository.GetByIdAsync(id);
        if (existingTodo is null)
        {
            return TypedResults.NotFound();
        }

        var updatedTodo = new Todo
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsCompleted,
            DueDate = request.DueDate,
            CreatedAt = existingTodo.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
        };

        var success = await repository.UpdateAsync(updatedTodo);
        return success ? TypedResults.NoContent() : TypedResults.Problem("Failed to update todo");
    }

    private static async Task<Results<NoContent, NotFound>> DeleteTodo(
        int id,
        ITodoRepository repository
    )
    {
        var success = await repository.DeleteAsync(id);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
