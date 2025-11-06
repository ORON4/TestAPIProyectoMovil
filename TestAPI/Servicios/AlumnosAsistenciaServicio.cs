using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestAPI.Modelos;
using TestAPI.Repositorios;
using TestAPI.Exceptions;

namespace TestAPI.Servicios
{
    public interface IAlumnosAsistenciaServicio
    {
        Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas();

        Task<AlumnosAsistencia>CrearAlumnosAsistencia(AlumnosAsistencia alumnosAsistencia);

        Task<bool> EliminarAlumnosAsistencia(int id);

    }

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

       

        public async Task<AlumnosAsistencia> CrearAlumnosAsistencia(AlumnosAsistencia alumnosAsistencia)
        {
            var newId = await _alumnosAsistenciaRepositorio.CrearAlumnosAsistencia(alumnosAsistencia);
            alumnosAsistencia.Id = newId;
            return alumnosAsistencia;
        }

        public async Task<bool> EliminarAlumnosAsistencia(int id)
        {
            return await _alumnosAsistenciaRepositorio.EliminarAlumnosAsistencia(id);
        }



    }
}
