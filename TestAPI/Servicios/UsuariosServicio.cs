using TestAPI.Modelos;
using TestAPI.Repositorios;

namespace TestAPI.Servicios
{
    public interface IUsuarioServicio
    {
        Task<IEnumerable<Usuarios>> ObtenerTodas();
        Task<Usuarios?> ObtenerPorId(int id);
        Task<IEnumerable<Usuarios>> ObtenerPorEstatus(int estatus);
        Task<Usuarios> CrearUsuario(Usuarios usuario);
        Task<bool> EliminarUsuario(int id);
    }

    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IEnumerable<Usuarios>> ObtenerTodas()
        {
            return await _usuarioRepositorio.ObtenerTodas();
        }

        public async Task<Usuarios?> ObtenerPorId(int id)
        {
            return await _usuarioRepositorio.ObtenerPorId(id);
        }

        public async Task<IEnumerable<Usuarios>> ObtenerPorEstatus(int estatus)
        {
            return await _usuarioRepositorio.ObtenerPorEstatus(estatus);
        }

        public async Task<Usuarios> CrearUsuario(Usuarios usuario)
        {
            var newId = await _usuarioRepositorio.CrearUsuario(usuario);
            usuario.Id = newId;
            return usuario;
        }

        public async Task<bool> EliminarUsuario(int id)
        {
            return await _usuarioRepositorio.EliminarUsuario(id);
        }
    }
}
