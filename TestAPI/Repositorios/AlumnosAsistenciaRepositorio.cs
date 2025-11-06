using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;

namespace TestAPI.Repositorios
{
    public interface IAlumnosAsistenciaRepositorio
    {
        Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas();
        Task<int> CrearAlumnosAsistencia(AlumnosAsistencia alumnosAsistencia);
        Task<bool> EliminarAlumnosAsistencia(int id);
    }

    public class AlumnosAsistenciaRepositorio : IAlumnosAsistenciaRepositorio
    {
        private readonly string _connectionString;

        public AlumnosAsistenciaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas()
        {
            var conexion = new SqlConnection(_connectionString);

            return await conexion.QueryAsync<AlumnosAsistencia>("SELECT * FROM AlumnosAsistencia");
        }

        public async Task<int> CrearAlumnosAsistencia(AlumnosAsistencia alumnosAsistencia)
        {
            using var conexion = new SqlConnection(_connectionString);

            var sql = @"
                INSERT INTO AlumnosAsistencia (NombreAlumno, Grupo, Fecha, Asistencia)
                VALUES (@NombreAlumno, @Grupo, @Fecha, @Asistencia);

               
            ";

            var nuevoid = await conexion.ExecuteAsync(sql, new
            {
                alumnosAsistencia.NombreAlumno,
                alumnosAsistencia.Grupo,
                alumnosAsistencia.Fecha,
                alumnosAsistencia.Asistencia
            });


            

            return nuevoid;
        }

        public async Task<bool> EliminarAlumnosAsistencia(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            var rows = await conexion.ExecuteAsync("DELETE FROM AlumnosAsistencia WHERE Id = @Id", new { Id = id });
            return rows > 0;
        }


    }
}
