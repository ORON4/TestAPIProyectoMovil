using TestAPI.Modelos;
using TestAPI.Repositorios;

namespace TestAPI.Servicios
{
    public class AlumnoServicio
    {
        public class AlumnosServicio : IAlumnosServicio
        {
            private readonly IAlumnosRepositorio _alumnosRepositorio;

            public AlumnosServicio(IAlumnosRepositorio alumnosRepositorio)
            {
                _alumnosRepositorio = alumnosRepositorio;
            }

            public async Task<IEnumerable<Alumno>> ObtenerPorGrupo(int idGrupo)
            {
                return await _alumnosRepositorio.ObtenerPorGrupo(idGrupo);
            }

            public async Task<Alumno> ObtenerPorId(int id)
            {
                return await _alumnosRepositorio.ObtenerPorId(id);
            }
        }
    }
}
