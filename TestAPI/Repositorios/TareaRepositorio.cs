using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;

namespace TestAPI.Repositorios
{
    public interface ITareaRepositorio
    {
        // Métodos modificados/nuevos
        Task<IEnumerable<TareaAlumnoDTO>> ObtenerPorAlumno(int alumnoId);
        Task<int> CrearTareaParaTodos(Tarea tarea);
        Task<bool> MarcarEntregada(int alumnoTareaId);
        Task<bool> EliminarTareaDeAlumno(int alumnoTareaId);

        // Métodos para mantenimiento (opcionales, editar la definición global)
        Task<bool> ActualizarDefinicionTarea(Tarea tarea);
        Task<Tarea?> ObtenerDefinicionPorId(int id);
    }

    public class TareaRepositorio : ITareaRepositorio
    {
        private readonly string _connectionString;

        public TareaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // 1. Obtener tareas específicas de un alumno (JOIN)
        public async Task<IEnumerable<TareaAlumnoDTO>> ObtenerPorAlumno(int alumnoId)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    T.Id, 
                    AT.Id AS AlumnoTareaId,
                    T.Titulo, 
                    T.Descripcion, 
                    T.FechaEntrega, 
                    AT.Estatus
                FROM Tareas T
                INNER JOIN AlumnoTareas AT ON T.Id = AT.TareaId
                WHERE AT.AlumnoId = @AlumnoId";

            return await conexion.QueryAsync<TareaAlumnoDTO>(sql, new { AlumnoId = alumnoId });
        }

        // 2. Crear Tarea y asignarla a TODOS los alumnos
        public async Task<int> CrearTareaParaTodos(Tarea tarea)
        {
            using var conexion = new SqlConnection(_connectionString);
            await conexion.OpenAsync();

            // Usamos una transacción para asegurar que se haga todo o nada
            using var transaction = conexion.BeginTransaction();
            try
            {
                // Paso A: Insertar la Tarea Global
                var sqlTarea = @"
                    INSERT INTO Tareas (Titulo, Descripcion, FechaEntrega)
                    OUTPUT INSERTED.Id
                    VALUES (@Titulo, @Descripcion, @FechaEntrega);";

                var tareaId = await conexion.ExecuteScalarAsync<int>(sqlTarea, new
                {
                    tarea.Titulo,
                    tarea.Descripcion,
                    tarea.FechaEntrega
                }, transaction);

                // Paso B: Asignar esa tarea a TODOS los alumnos existentes
                var sqlAsignar = @"
                    INSERT INTO AlumnoTareas (AlumnoId, TareaId, Estatus)
                    SELECT Id, @TareaId, 0 
                    FROM Alumnos;";

                await conexion.ExecuteAsync(sqlAsignar, new { TareaId = tareaId }, transaction);

                transaction.Commit();
                return tareaId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // 3. Marcar tarea como entregada (Usando el ID de la tabla intermedia)
        public async Task<bool> MarcarEntregada(int alumnoTareaId)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = "UPDATE AlumnoTareas SET Estatus = 1 WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, new { Id = alumnoTareaId });
            return filas > 0;
        }

        // 4. Eliminar tarea de un alumno específico (ocultarla para él)
        public async Task<bool> EliminarTareaDeAlumno(int alumnoTareaId)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = "DELETE FROM AlumnoTareas WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, new { Id = alumnoTareaId });
            return filas > 0;
        }

        // Mantenimiento: Editar el texto de la tarea global
        public async Task<bool> ActualizarDefinicionTarea(Tarea tarea)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE Tareas
                SET Titulo = @Titulo,
                    Descripcion = @Descripcion,
                    FechaEntrega = @FechaEntrega
                WHERE Id = @Id;";

            var filas = await conexion.ExecuteAsync(sql, tarea);
            return filas > 0;
        }

        public async Task<Tarea?> ObtenerDefinicionPorId(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            return await conexion.QueryFirstOrDefaultAsync<Tarea>("SELECT * FROM Tareas WHERE Id = @Id", new { Id = id });
        }
    }
}