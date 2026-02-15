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

    /// <summary>
    /// Checks if the StockQuantity column exists in the Products table and adds it if not.
    /// </summary>
    public static void EnsureStockColumn()
    {
        string query = @"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'StockQuantity')
            BEGIN
                ALTER TABLE Products ADD StockQuantity INT NOT NULL DEFAULT 0;
                EXEC('UPDATE Products SET StockQuantity = 50'); -- Set initial stock to 50 for existing items
            END";
        ExecuteNonQuery(query);
    }

    /// <summary>
    /// Checks if the StockQuantity column exists in the Herbs (Ingredients) table and adds it if not.
    /// </summary>
    public static void EnsureHerbStockColumn()
    {
        string query = @"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Herbs' AND COLUMN_NAME = 'StockQuantity')
            BEGIN
                ALTER TABLE Herbs ADD StockQuantity INT NOT NULL DEFAULT 0;
                EXEC('UPDATE Herbs SET StockQuantity = 100'); -- Set initial stock to 100 for existing herbs
            END";
        ExecuteNonQuery(query);
    }
}
