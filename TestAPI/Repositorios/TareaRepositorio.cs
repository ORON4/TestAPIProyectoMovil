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
        Task<IEnumerable<Tarea>> ObtenerTodas();
        Task<Tarea?> ObtenerPorId(int id);
        Task<IEnumerable<Tarea>> ObtenerPorEstatus(int estatus);
        Task<int> CrearTarea(Tarea tarea);
        Task<bool> EliminarTarea(int id);
        Task<bool> ActualizarTarea(Tarea tarea);
    }

    public class TareaRepositorio : ITareaRepositorio
    {
        private readonly string _connectionString;

        public TareaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<IEnumerable<Tarea>> ObtenerTodas()
        {
            var conexion = new SqlConnection(_connectionString);

            return await conexion.QueryAsync<Tarea>("SELECT * FROM Tareas");
        }

        public async Task<Tarea?> ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            return await conexion.QueryFirstOrDefaultAsync<Tarea>(
                "SELECT * FROM Tareas WHERE Id = @Id",
                new { Id = id });
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorEstatus(int estatus)
        {
            using var conexion = new SqlConnection(_connectionString);
            return await conexion.QueryAsync<Tarea>(
                "SELECT * FROM Tareas WHERE Estatus = @Estatus",
                new { Estatus = estatus });
        }

        public async Task<int> CrearTarea(Tarea tarea)
        {
            using var conexion = new SqlConnection(_connectionString);

            var sql = @"
                INSERT INTO Tareas (Titulo, Descripcion, FechaEntrega, Estatus)
                VALUES (@Titulo, @Descripcion, @FechaEntrega, @Estatus);

               
            ";

            var id = await conexion.ExecuteAsync(sql, new
            {
                tarea.Titulo,
                tarea.Descripcion,
                tarea.FechaEntrega,
                tarea.Estatus
            });


            if (id > 0)
                id = tarea.Id;

            return id;
        }

        public async Task<bool> EliminarTarea(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            var rows = await conexion.ExecuteAsync("DELETE FROM Tareas WHERE Id = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ActualizarTarea(Tarea tarea)
        {
            using var conexion = new SqlConnection(_connectionString);

            var sql = @"
            UPDATE Tareas
            SET Titulo = @Titulo,
                Descripcion = @Descripcion,
                FechaEntrega = @FechaEntrega,
                Estatus = @Estatus
            WHERE Id = @Id;
        ";

            var filasAfectadas = await conexion.ExecuteAsync(sql, new
            {
                tarea.Titulo,
                tarea.Descripcion,
                tarea.FechaEntrega,
                tarea.Estatus,
                tarea.Id  // Asegúrate de pasar el Id para el WHERE
            });

            // ExecuteAsync devuelve el número de filas afectadas.
            // Si es mayor a 0, la actualización fue exitosa.
            return filasAfectadas > 0;
        }


    }
}
