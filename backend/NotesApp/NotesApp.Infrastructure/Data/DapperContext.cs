using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NotesApp.Infrastructure.Data;

/// <summary>
/// Provides SQL Server database connections via Dapper.
/// </summary>
public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
