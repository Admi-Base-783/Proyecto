using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Proyecto.Data
{
    public static class DatabaseExecutor
    {
        // Configuración para alternar entre SQL Server y MySQL
        private static readonly string sqlServerConnectionString = "Server=DESKTOP-2BF3DDU\\SQLEXPRESS;Database=Proyecto;Trusted_Connection=True;";
        private static readonly string mySqlConnectionString = "Server=localhost;Database=Proyecto;User=root;Password=;";

        // Cambiar a `true` para usar MySQL, `false` para usar SQL Server
        public static bool useMySql = false;

        // Método para ejecutar consultas SQL sin resultados (INSERT, UPDATE, DELETE, etc.)
        public static void ExecuteNonQuerySql(string query)
        {
            if (useMySql)
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(sqlServerConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Método para ejecutar consultas SQL sin resultados (para compatibilidad con el controlador)
        public static void ExecuteSqlQuery(string query)
        {
            ExecuteNonQuerySql(query);
        }

        // Método para obtener las tablas de la base de datos
        public static List<string> GetTables()
        {
            var tables = new List<string>();
            string query = useMySql
                ? "SHOW TABLES"
                : "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            if (useMySql)
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tables.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(sqlServerConnectionString))
                {
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
            }

            return tables;
        }

        // Método para ejecutar consultas que devuelven resultados (SELECT)
        public static DataTable ExecuteSqlQueryWithResult(string query)
        {
            DataTable resultTable = new DataTable();

            if (useMySql)
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(resultTable);
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(sqlServerConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(resultTable);
                        }
                    }
                }
            }

            return resultTable;
        }

        // Método para obtener columnas de una tabla
        public static List<string> GetColumnsFromTable(string tableName)
        {
            var columns = new List<string>();
            string query = useMySql
                ? $"SHOW COLUMNS FROM {tableName}"
                : $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

            if (useMySql)
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                columns.Add(reader["Field"].ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(sqlServerConnectionString))
                {
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
            }

            return columns;
        }
    }
}


