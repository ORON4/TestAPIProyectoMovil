using TestAPI.Modelos;

namespace TestAPI.Servicios
{

        public interface IAlumnosServicio
        {
            Task<IEnumerable<Alumno>> ObtenerPorGrupo(int idGrupo);
        }
}

