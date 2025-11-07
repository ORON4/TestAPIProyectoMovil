using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;
using TestAPI.Repositorios;

namespace TestAPI.Servicios
{
    public class AlumnosAsistenciaServicio : IAlumnosAsistenciaServicio
    {
        private readonly IAlumnosAsistenciaRepositorio _alumnosAsistenciaRepositorio;

        public AlumnosAsistenciaServicio(IAlumnosAsistenciaRepositorio alumnosAsistenciaRepositorio)
        {
            
            _alumnosAsistenciaRepositorio = alumnosAsistenciaRepositorio;
        }

        public async Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas()
        {
            return await _alumnosAsistenciaRepositorio.ObtenerTodas();
        }

        public async Task<bool> GuardarAsistencia(AlumnosAsistencia alumnosAsistencia)
        {
            return await _alumnosAsistenciaRepositorio.GuardarAsistencia(alumnosAsistencia);
        }

        public async Task<bool> EliminarAlumnosAsistencia(int id)
        {
            return await _alumnosAsistenciaRepositorio.EliminarAlumnosAsistencia(id);
        }

        public async Task<IEnumerable<AsistenciaReporte>> ObtenerPorFecha(DateTime fecha)
        {
            return await _alumnosAsistenciaRepositorio.ObtenerPorFecha(fecha);
        }
    }
}