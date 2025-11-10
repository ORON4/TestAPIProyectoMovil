using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;

namespace TestAPI.Repositorios
{
    public class AlumnosRepositorio : IAlumnosRepositorio
    {
        private readonly string _connectionString;

        public AlumnosRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Alumno>> ObtenerPorGrupo(int idGrupo)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Alumnos WHERE Grupo = @IdGrupo";
            return await conexion.QueryAsync<Alumno>(sql, new { IdGrupo = idGrupo });
        }

        public async Task<Alumno> ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Alumnos WHERE Id = @Id";
            return await conexion.QueryFirstOrDefaultAsync<Alumno>(sql, new { Id = id });
        }
    }
}