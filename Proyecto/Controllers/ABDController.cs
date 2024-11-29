using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Proyecto.Models;
using Proyecto.Data;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace Proyecto.Controllers
{
    public class ABDController : Controller
    {
        // GET: ABD
        public ActionResult Opciones()
        {
            return View();
        }

        public ActionResult CrearTabla()
        {
            try
            {
                ViewBag.Tables = DatabaseExecutor.GetTables();
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al cargar las tablas: {ex.Message}";
                TempData["MessageType"] = "error";
                ViewBag.Tables = new List<string>(); // Evitar errores adicionales
            }

            return View();
        }
        public ActionResult CrearRelaciones()
        {
            try
            {
                // Llama al método para obtener las tablas y pásalas a la vista
                ViewBag.Tables = DatabaseExecutor.GetTables();
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje amigable
                TempData["Message"] = $"Error al cargar las tablas: {ex.Message}";
                TempData["MessageType"] = "error";
                ViewBag.Tables = new List<string>(); // Evitar errores adicionales
            }

            return View();
        }

        public ActionResult EjecutarSentencia()
        {
            var model = new SqlQueryModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult EjecutarSentencia(SqlQueryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Query))
                {
                    TempData["Message"] = "La consulta SQL no puede estar vacía.";
                    TempData["MessageType"] = "error"; // Tipo de mensaje para aplicar estilos
                    return View(model);
                }

                if (model.Query.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                {
                    var result = DatabaseExecutor.ExecuteSqlQueryWithResult(model.Query);
                    model.Result = GenerateHtmlTable(result); // Convierte el resultado en una tabla HTML
                    TempData["Message"] = "Consulta ejecutada correctamente. Resultado disponible.";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    DatabaseExecutor.ExecuteNonQuerySql(model.Query); // Ejecuta otras consultas (INSERT, UPDATE, DELETE)
                    TempData["Message"] = "Operación realizada con éxito.";
                    TempData["MessageType"] = "success";
                }
            }
            catch (SqlException ex)
            {
                TempData["Message"] = $"Error en SQL: {ex.Message}";
                TempData["MessageType"] = "error";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error inesperado: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return View(model);
        }

        // Método para convertir DataTable en HTML
        private string GenerateHtmlTable(DataTable dataTable)
        {
            string html = "<table class='table table-bordered'>";
            html += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                html += $"<th>{column.ColumnName}</th>";
            }
            html += "</tr></thead>";

            html += "<tbody>";
            foreach (DataRow row in dataTable.Rows)
            {
                html += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    html += $"<td>{item}</td>";
                }
                html += "</tr>";
            }
            html += "</tbody></table>";

            return html;
        }



        private string DataTableToHtml(System.Data.DataTable table)
        {
            string html = "<table class='table table-bordered'>";
            html += "<tr>";
            foreach (System.Data.DataColumn column in table.Columns)
            {
                html += $"<th>{column.ColumnName}</th>";
            }
            html += "</tr>";

            foreach (System.Data.DataRow row in table.Rows)
            {
                html += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    html += $"<td>{item}</td>";
                }
                html += "</tr>";
            }

            html += "</table>";
            return html;
        }


        [HttpPost]
        public ActionResult CrearTablaDinamica(TableDefinitionModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string sqlQuery = SqlQueryBuilder.GenerateCreateTableQuery(model);
                    DatabaseExecutor.ExecuteSqlQuery(sqlQuery);

                    // Crear la llave foránea si está definida
                    if (model.ForeignKey != null && !string.IsNullOrWhiteSpace(model.ForeignKey.SourceColumn))
                    {
                        string foreignKeyQuery = SqlQueryBuilder.GenerateForeignKeyQuery(model.ForeignKey, model.TableName);
                        DatabaseExecutor.ExecuteSqlQuery(foreignKeyQuery);
                    }

                    TempData["Message"] = "¡Tabla creada exitosamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Error al crear la tabla: {ex.Message}";
                    TempData["MessageType"] = "error";
                }

                return RedirectToAction("CrearTabla");
            }

            TempData["Message"] = "El modelo enviado no es válido.";
            TempData["MessageType"] = "error";
            return RedirectToAction("CrearTabla");
        }
        [HttpGet]
        public JsonResult GetColumns(string tableName)
        {
            try
            {
                var columns = DatabaseExecutor.GetColumnsFromTable(tableName);
                return Json(columns, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CrearRelacion(RelationshipDefinitionModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "El modelo enviado no es válido. Verifique los datos.";
                TempData["MessageType"] = "error";
                return RedirectToAction("CrearRelaciones");
            }

            // Validar si las tablas y columnas existen
            var sourceColumns = DatabaseExecutor.GetColumnsFromTable(model.SourceTable);
            var targetColumns = DatabaseExecutor.GetColumnsFromTable(model.TargetTable);

            if (!sourceColumns.Contains(model.SourceColumn) || !targetColumns.Contains(model.TargetColumn))
            {
                TempData["Message"] = "La columna seleccionada no existe en la tabla correspondiente.";
                TempData["MessageType"] = "error";
                return RedirectToAction("CrearRelaciones");
            }

            // Ejecutar la creación de la relación
            try
            {
                string sqlQuery = SqlQueryBuilder.GenerateRelationshipQuery(model);
                DatabaseExecutor.ExecuteSqlQuery(sqlQuery);

                TempData["Message"] = "¡Relación creada exitosamente!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al crear la relación: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("CrearRelaciones");
        }

    }
}