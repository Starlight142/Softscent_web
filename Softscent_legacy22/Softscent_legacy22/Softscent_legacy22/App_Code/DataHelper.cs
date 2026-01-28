using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Softscent.Data;
using Softscent.Models;

/// <summary>
/// Simple Data Helper to manage database connections and execute SQL commands.
/// Replaces the need for a full ORM like EF Core for this legacy context.
/// MODIFIED: Uses MockDataStore instead of SQL Server.
/// </summary>
public static class DataHelper
{
    /// <summary>
    /// Executes a SELECT query and returns the results as a DataTable.
    /// </summary>
    public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
    {
        string cleanQuery = query.ToUpper().Trim();

        // -----------------------
        // PRODUCTS
        // -----------------------
        if (cleanQuery.Contains("FROM PRODUCTS"))
        {
            var dt = MockDataStore.Database.Tables["Products"];
            
            // Handle ID lookup: SELECT * FROM Products WHERE Id = @Id
            if (parameters != null && parameters.ContainsKey("@Id"))
            {
                 int id = Convert.ToInt32(parameters["@Id"]);
                 var rows = dt.AsEnumerable().Where(r => r["Id"] != DBNull.Value && Convert.ToInt32(r["Id"]) == id);
                 if (rows.Any()) return rows.CopyToDataTable();
                 return dt.Clone();
            }

            // Handle specific Name lookup (Cart.aspx.cs uses hardcoded string)
            // Query: SELECT * FROM Products WHERE Name = 'Custom Inhaler Blend'
            // MockDataStore uses "Custom Blend Inhaler" so we map it.
            if (cleanQuery.Contains("WHERE NAME = 'CUSTOM INHALER BLEND'") || cleanQuery.Contains("WHERE NAME = 'CUSTOM BLEND INHALER'"))
            {
                 var rows = dt.AsEnumerable().Where(r => r["IsCustomizable"] != DBNull.Value && (bool)r["IsCustomizable"]);
                 if (rows.Any()) return rows.CopyToDataTable();
                 return dt.Clone();
            }
            
            // Handle Search: WHERE Name LIKE @Search OR Description LIKE @Search
            if (parameters != null && parameters.ContainsKey("@Search"))
            {
                string search = parameters["@Search"].ToString().Replace("%", "").ToLower();
                var filteredRows = dt.AsEnumerable().Where(r => 
                    (r["Name"] != DBNull.Value && r["Name"].ToString().ToLower().Contains(search)) || 
                    (r["Description"] != DBNull.Value && r["Description"].ToString().ToLower().Contains(search))
                );
                
                if (filteredRows.Any()) return filteredRows.CopyToDataTable();
                return dt.Clone();
            }

            return dt.Copy();
        }

        // -----------------------
        // USERS
        // -----------------------
        if (cleanQuery.Contains("FROM USERS"))
        {
             var dt = MockDataStore.Database.Tables["Users"];
             
             // Handler for: SELECT * FROM Users WHERE Email = @Email [AND PasswordHash = @Password]
             if (parameters != null && parameters.ContainsKey("@Email"))
             {
                 string email = parameters["@Email"].ToString();
                 
                 // Login Check with Password
                 if (parameters.ContainsKey("@Password")) 
                 {
                     string pass = parameters["@Password"].ToString();
                     var rows = dt.AsEnumerable().Where(r => 
                        r["Email"].ToString().Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) && 
                        r["PasswordHash"].ToString() == pass
                     );
                     
                     if (rows.Any()) return rows.CopyToDataTable();
                     return dt.Clone();
                 }
                 
                 // Registration / Lookup Check (just Email)
                 var emailRows = dt.AsEnumerable().Where(r => 
                    r["Email"].ToString().Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)
                 );
                 if (emailRows.Any()) return emailRows.CopyToDataTable();
                 return dt.Clone();
             }
             
             return dt.Copy();
        }

        // -----------------------
        // ROLES (Admin Check)
        // -----------------------
        if (cleanQuery.Contains("FROM ROLES") || cleanQuery.Contains("JOIN USERROLES"))
        {
             // Mock Admin check: if userId = '1' return 'Admin'
             if (parameters != null && parameters.ContainsKey("@UserId"))
             {
                 string uid = parameters["@UserId"].ToString();
                 if (uid == "1") 
                 {
                     DataTable t = new DataTable();
                     t.Columns.Add("Name");
                     t.Rows.Add("Admin");
                     return t;
                 }
             }
             return new DataTable();
        }

        // -----------------------
        // NEWS
        // -----------------------
        if (cleanQuery.Contains("FROM NEWS"))
        {
            return MockDataStore.Database.Tables["News"].Copy();
        }

        // -----------------------
        // REVIEWS
        // -----------------------
        if (cleanQuery.Contains("FROM REVIEWS"))
        {
            var dt = MockDataStore.Database.Tables["Reviews"];
            // Simple filter by ProductId if needed
             if (parameters != null && parameters.ContainsKey("@ProductId"))
             {
                 int pid = Convert.ToInt32(parameters["@ProductId"]);
                 var rows = dt.AsEnumerable().Where(r => Convert.ToInt32(r["ProductId"]) == pid);
                 if (rows.Any()) return rows.CopyToDataTable();
                 return dt.Clone();
             }
            return dt.Copy();
        }

        // Fallback for unhandled queries
        return new DataTable();
    }

    /// <summary>
    /// Executes an INSERT, UPDATE, or DELETE query.
    /// </summary>
    public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
         string cleanQuery = query.ToUpper().Trim();
         
         // -----------------------
         // INSERT USERS (Registration)
         // -----------------------
         if (cleanQuery.StartsWith("INSERT INTO USERS"))
         {
             var dt = MockDataStore.Database.Tables["Users"];
             DataRow row = dt.NewRow();
             
             // Mapping standard params
             if (parameters != null) {
                 row["Id"] = Guid.NewGuid().ToString();
                 if (parameters.ContainsKey("@Email")) row["Email"] = parameters["@Email"];
                 if (parameters.ContainsKey("@Password")) row["PasswordHash"] = parameters["@Password"];
                 if (parameters.ContainsKey("@FullName")) row["FullName"] = parameters["@FullName"];
                 
                 // Defaults
                 row["UserName"] = parameters.ContainsKey("@Email") ? parameters["@Email"] : "";
                 row["NormalizedUserName"] = parameters.ContainsKey("@Email") ? parameters["@Email"].ToString().ToUpper() : "";
                 row["EmailConfirmed"] = true;
                 row["PhoneNumberConfirmed"] = false;
                 row["TwoFactorEnabled"] = false;
                 row["LockoutEnabled"] = true;
                 row["AccessFailedCount"] = 0;
                 row["SecurityStamp"] = Guid.NewGuid().ToString();
             }
             dt.Rows.Add(row);
             return 1;
         }

          // -----------------------
          // INSERT ORDER DETAILS
          // -----------------------
          if (cleanQuery.StartsWith("INSERT INTO ORDERDETAILS"))
         {
             var dt = MockDataStore.Database.Tables["OrderDetails"];
             DataRow row = dt.NewRow();
              if (parameters != null) {
                 row["OrderId"] = parameters["@OrderId"];
                 row["ProductId"] = parameters["@ProductId"];
                 row["Quantity"] = parameters["@Quantity"];
                 row["UnitPrice"] = parameters["@UnitPrice"];
                 row["CustomConfiguration"] = parameters.ContainsKey("@CustomConfig") ? parameters["@CustomConfig"] : DBNull.Value;
             }
             dt.Rows.Add(row);
             return 1;
         }
         
         // -----------------------
         // REVIEWS / SUPPORT (Simple Inserts)
         // -----------------------
         if (cleanQuery.StartsWith("INSERT INTO REVIEWS"))
         {
             var dt = MockDataStore.Database.Tables["Reviews"];
             DataRow row = dt.NewRow();
             if (parameters != null) {
                // Assuming params match map roughly
                if (parameters.ContainsKey("@ProductId")) row["ProductId"] = parameters["@ProductId"];
                if (parameters.ContainsKey("@UserId")) row["UserId"] = parameters["@UserId"];
                if (parameters.ContainsKey("@Rating")) row["Rating"] = parameters["@Rating"];
                if (parameters.ContainsKey("@Comment")) row["Comment"] = parameters["@Comment"];
                row["CreatedDate"] = DateTime.Now;
             }
             dt.Rows.Add(row);
             return 1;
         }

        return 0;
    }

    /// <summary>
    /// Executes a query and returns the first column of the first row (or specifically ID for Insert).
    /// </summary>
    public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
    {
        string cleanQuery = query.ToUpper().Trim();

        // -----------------------
        // INSERT ORDERS (Return ID)
        // -----------------------
        if (cleanQuery.StartsWith("INSERT INTO ORDERS"))
        {
             var dt = MockDataStore.Database.Tables["Orders"];
             DataRow row = dt.NewRow();
             
             if (parameters != null) {
                 row["UserId"] = parameters["@UserId"];
                 row["OrderDate"] = parameters.ContainsKey("@OrderDate") ? parameters["@OrderDate"] : DateTime.Now;
                 row["TotalAmount"] = parameters["@TotalAmount"];
                 row["Status"] = parameters["@Status"];
                 row["ShippingAddress"] = parameters.ContainsKey("@Address") ? parameters["@Address"] : DBNull.Value;
                 row["ShippingMethod"] = parameters.ContainsKey("@ShippingMethod") ? parameters["@ShippingMethod"] : DBNull.Value;
                 row["PaymentMethod"] = parameters.ContainsKey("@PaymentMethod") ? parameters["@PaymentMethod"] : DBNull.Value;
                 row["PaymentStatus"] = parameters.ContainsKey("@PaymentStatus") ? parameters["@PaymentStatus"] : DBNull.Value;
             }
             dt.Rows.Add(row);
             return row["Id"]; // Return the auto-generated ID
        }

        // -----------------------
        // SELECT COUNT / SELECT SINGLE VALUE
        // -----------------------
        // Fallback: Use ExecuteQuery and return first cell
        DataTable dtRes = ExecuteQuery(query, parameters);
        if (dtRes.Rows.Count > 0)
        {
            return dtRes.Rows[0][0];
        }

        return null;
    }
}
