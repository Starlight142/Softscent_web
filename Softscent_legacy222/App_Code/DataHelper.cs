using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Softscent.Models;

/// <summary>
/// Simple Data Helper to manage database connections and execute SQL commands.
/// Replaces the need for a full ORM like EF Core for this legacy context.
/// </summary>
public static class DataHelper
{
    // Retrieve the connection string from Web.config
    private static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; }
    }

    /// <summary>
    /// Executes a SELECT query and returns the results as a DataTable.
    /// </summary>
    /// <param name="query">The SQL query string.</param>
    /// <param name="parameters">Optional dictionary of parameters to prevent SQL injection.</param>
    /// <returns>A DataTable containing the query results.</returns>
    public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value ?? DBNull.Value);
                    }
                }
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    /// <summary>
    /// Executes an INSERT, UPDATE, or DELETE query.
    /// </summary>
    /// <param name="query">The SQL non-query command.</param>
    /// <param name="parameters">Optional dictionary of parameters.</param>
    /// <returns>The number of rows affected.</returns>
    public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value ?? DBNull.Value);
                    }
                }
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Executes a query and returns the first column of the first row in the result set.
    /// Useful for SELECT COUNT(*) or retrieving an ID.
    /// </summary>
    /// <param name="query">The SQL scalar query.</param>
    /// <param name="parameters">Optional dictionary of parameters.</param>
    /// <returns>The first column of the first row in the result set, or null if empty.</returns>
    public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value ?? DBNull.Value);
                    }
                }
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
