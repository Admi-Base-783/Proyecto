using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto.Data
{
    public static class DatabaseExecutor
    {
        private static readonly string connectionString = "Server=DESKTOP-2BF3DDU\\SQLEXPRESS;Database=Proyecto;Trusted_Connection=True;";
        
        // Método para ejecutar consultas SQL sin resultados (INSERT, UPDATE, DELETE, etc.)
        public static void ExecuteNonQuerySql(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); // Ejecuta la consulta
                }
            }
        }

        public static void ExecuteSqlQuery(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<string> GetTables()
        {
            var tables = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["TABLE_NAME"].ToString());
                        }
                    }
                }
            }
            return tables;
        }

        // Método para ejecutar consultas que devuelven resultados (ya que se menciona en tu código)
        public static DataTable ExecuteSqlQueryWithResult(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);
                        return resultTable;
                    }
                }
            }
        }

        // Método ejemplo para obtener columnas (ya que se usa en el código)
        public static List<string> GetColumnsFromTable(string tableName)
        {
            var columns = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader["COLUMN_NAME"].ToString());
                        }
                    }
                }
            }
            return columns;
        }
    }
}
