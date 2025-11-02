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
                INSERT INTO Tareas (Id, Titulo, Descripcion, FechaEntrega, Estatus)
                VALUES (@Id, @Titulo, @Descripcion, @FechaEntrega, @Estatus);
               
            ";

            var id = await conexion.ExecuteAsync(sql, new
            {
                tarea.Id,
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


    }
}
