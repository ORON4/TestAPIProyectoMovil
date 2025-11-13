using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;
using TestAPI.Repositorios;

namespace TestAPI.Servicios
{
    public interface ITareaServicio
    {
        Task<IEnumerable<TareaAlumnoDTO>> ObtenerPorAlumno(int alumnoId);
        Task<Tarea> CrearTareaParaTodos(Tarea tarea);
        Task<bool> MarcarEntregada(int alumnoTareaId);
        Task<bool> EliminarTareaDeAlumno(int alumnoTareaId);
        Task<bool> ActualizarDefinicion(Tarea tarea);
        Task<Tarea?> ObtenerDefinicion(int id);
    }

    public class TareaServicio : ITareaServicio
    {
        private readonly ITareaRepositorio _tareaRepositorio;

        public TareaServicio(ITareaRepositorio tareaRepositorio)
        {
            _tareaRepositorio = tareaRepositorio;
        }

        public async Task<IEnumerable<TareaAlumnoDTO>> ObtenerPorAlumno(int alumnoId)
        {
            return await _tareaRepositorio.ObtenerPorAlumno(alumnoId);
        }

        public async Task<Tarea> CrearTareaParaTodos(Tarea tarea)
        {
            var newId = await _tareaRepositorio.CrearTareaParaTodos(tarea);
            tarea.Id = newId;
            return tarea;
        }

        public async Task<bool> MarcarEntregada(int alumnoTareaId)
        {
            return await _tareaRepositorio.MarcarEntregada(alumnoTareaId);
        }

        public async Task<bool> EliminarTareaDeAlumno(int alumnoTareaId)
        {
            return await _tareaRepositorio.EliminarTareaDeAlumno(alumnoTareaId);
        }

        public async Task<bool> ActualizarDefinicion(Tarea tarea)
        {
            return await _tareaRepositorio.ActualizarDefinicionTarea(tarea);
        }

        public async Task<Tarea?> ObtenerDefinicion(int id)
        {
            return await _tareaRepositorio.ObtenerDefinicionPorId(id);
        }
    }
}