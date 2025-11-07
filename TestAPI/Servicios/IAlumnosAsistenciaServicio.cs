using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;

namespace TestAPI.Servicios
{
    // Esta interfaz define "qué" debe hacer el servicio
    public interface IAlumnosAsistenciaServicio
    {
        Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas();

        Task<bool> GuardarAsistencia(AlumnosAsistencia alumnosAsistencia);

        Task<bool> EliminarAlumnosAsistencia(int id);
    }
}