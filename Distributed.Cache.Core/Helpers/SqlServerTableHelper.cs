using System.Data;
using Microsoft.Data.SqlClient;

namespace Distributed.Cache.Core.Helpers;

public static class SqlServerTableHelper
{
    private static string SqlServerTableExistQuery(string instanceName) => $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = '{instanceName}'";

    private static string SqlServerTableCreateQuery(string instanceName) => 
        $"""
         CREATE TABLE [dbo].[{instanceName}](
                     [Id] [nvarchar](449) NOT NULL,
                     [Value] [varbinary](max) NOT NULL,
                     [ExpiresAtTime] [datetimeoffset](7) NOT NULL,
                     [SlidingExpirationInSeconds] [bigint] NULL,
                     [AbsoluteExpiration] [datetimeoffset](7) NULL,
                     PRIMARY KEY CLUSTERED ([Id] ASC))
         """;
    
    internal static void CreateSqlCacheIfNotExists(string connectionString, string instanceName)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        var command = new SqlCommand(SqlServerTableExistQuery(instanceName), connection);
        using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
        {
            if (reader.Read())
            {
                return;
            }
        }
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                command = new SqlCommand(SqlServerTableCreateQuery(instanceName), connection, transaction);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
    }
}