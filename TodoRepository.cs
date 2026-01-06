namespace SampleCrud.Repositories;

using System.Data;
using Dapper;
using SampleCrud.Models;

public class TodoRepository : ITodoRepository
{
    private readonly DbConnProvider _dbConnProvider;

    public TodoRepository(DbConnProvider dbConnProvider)
    {
        _dbConnProvider = dbConnProvider;
    }

    private IDbConnection CreateConnection() => _dbConnProvider.Connect();

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT Id, Title, Description, IsCompleted, DueDate, CreatedAt, UpdatedAt
            FROM sample.dbo.Todos
            ORDER BY CreatedAt DESC
            """;

        return await connection.QueryAsync<Todo>(sql);
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT Id, Title, Description, IsCompleted, DueDate, CreatedAt, UpdatedAt
            FROM sample.dbo.Todos
            WHERE Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Todo>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Todo todo)
    {
        using var connection = CreateConnection();
        const string sql = """
            INSERT INTO sample.dbo.Todos (Title, Description, IsCompleted, DueDate, CreatedAt)
            VALUES (@Title, @Description, @IsCompleted, @DueDate, SYSUTCDATETIME());
            SELECT CAST(SCOPE_IDENTITY() as int)
            """;

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                todo.Title,
                todo.Description,
                todo.IsCompleted,
                todo.DueDate,
            }
        );
    }

    public async Task<bool> UpdateAsync(Todo todo)
    {
        using var connection = CreateConnection();
        const string sql = """
            UPDATE sample.dbo.Todos
            SET Title = @Title,
                Description = @Description,
                IsCompleted = @IsCompleted,
                DueDate = @DueDate,
                UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @Id
            """;

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                todo.Id,
                todo.Title,
                todo.Description,
                todo.IsCompleted,
                todo.DueDate,
            }
        );

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = """DELETE FROM sample.dbo.Todos WHERE Id = @Id""";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}
