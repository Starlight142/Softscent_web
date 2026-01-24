using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Softscent.Models;

/// <summary>
/// Simple Data Helper to replace EF Core Context
/// </summary>
public static class DataHelper
{
    private static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; }
    }

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
