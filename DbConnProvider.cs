using System.Data;
using Microsoft.Data.SqlClient;

namespace SampleCrud;

/// <summary>
/// Provides connection to the database
/// </summary>
/// <param name="connectionString"></param>
public sealed class DbConnProvider(string connectionString)
{
    private readonly string _connectionString = connectionString;

    /// <summary>
    /// Factory method to provide connection to an SQL database.
    /// </summary>
    /// <returns>An instance implementing the <see cref="IDbConnection"/></returns>
    public IDbConnection Connect()
    {
        return new SqlConnection(_connectionString);
    }
}
