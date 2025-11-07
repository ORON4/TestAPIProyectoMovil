using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;

namespace TestAPI.Repositorios
{
    public interface IAlumnosAsistenciaRepositorio
    {
        Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas();

        // Modificamos el método de 'Crear' por 'Guardar' (UPSERT)
        Task<bool> GuardarAsistencia(AlumnosAsistencia alumnosAsistencia);

        // Mantengo este método por si lo tenías
        Task<bool> EliminarAlumnosAsistencia(int id);

        Task<IEnumerable<AsistenciaReporte>> ObtenerPorFecha(DateTime fecha);
    }
}