using Microsoft.Data.SqlClient;
using Dapper; // Agrega esta directiva para usar Dapper
using TestAPI.Modelos;

namespace TestAPI.Repositorios
{
    public interface IUsuarioRepositorio
    {
        Task<IEnumerable<Usuarios>> ObtenerTodas();
        Task<Usuarios?> ObtenerPorId(int id);
        Task<IEnumerable<Usuarios>> ObtenerPorEstatus(int estatus);
        Task<int> CrearUsuario(Usuarios usuario);
        Task<bool> EliminarUsuario(int id);
    }

    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<IEnumerable<Usuarios>> ObtenerTodas()
        {
            using var conexion = new SqlConnection(_connectionString);
            return await conexion.QueryAsync<Usuarios>("SELECT id, NombreUsuario,Contraseña, NombreCompleto From Usuarios");
        }

        public async Task<Usuarios?> ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            return await conexion.QueryFirstOrDefaultAsync<Usuarios>(
                "SELECT id, NombreUsuario, NombreCompleto FROM Usuarios WHERE Id = @Id",
                new { Id = id });
        }

        public async Task<IEnumerable<Usuarios>> ObtenerPorEstatus(int estatus)
        {
            using var conexion = new SqlConnection(_connectionString);
            return await conexion.QueryAsync<Usuarios>(
                "SELECT * FROM Usuarios WHERE Estatus = @Estatus",
                new { Estatus = estatus });
        }

        public async Task<int> CrearUsuario(Usuarios usuario)
        {
            using var conexion = new SqlConnection(_connectionString);

            var sql = @"
                INSERT INTO Usuarios (NombreUsuario, Contraseña, NombreCompleto)
                VALUES (@NombreUsuario, @Contraseña, @NombreCompleto);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var id = await conexion.ExecuteScalarAsync<int>(sql, new
            {   
                usuario.Id,
                usuario.NombreUsuario,
                usuario.Contraseña,
                usuario.NombreCompleto
            });

            return id;
        }

        public async Task<bool> EliminarUsuario(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            var rows = await conexion.ExecuteAsync("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}
