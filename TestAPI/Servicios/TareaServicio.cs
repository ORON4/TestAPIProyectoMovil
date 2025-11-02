using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestAPI.Modelos;
using TestAPI.Repositorios;
using TestAPI.Exceptions;

namespace TestAPI.Servicios
{
    public interface ITareaServicio
    {
        Task<IEnumerable<Tarea>> ObtenerTodas();
        Task<Tarea?> ObtenerPorId(int id);
        Task<IEnumerable<Tarea>> ObtenerPorEstatus(int estatus);
        Task<Tarea> CrearTarea(Tarea tarea);
        Task<bool> EliminarTarea(int id);
    }

    public class TareaServicio : ITareaServicio
    {
        private readonly ITareaRepositorio _tareaRepositorio;

        public TareaServicio(ITareaRepositorio tareaRepositorio)
        {
            _tareaRepositorio = tareaRepositorio;
        }

        public async Task<IEnumerable<Tarea>> ObtenerTodas()
        {
            return await _tareaRepositorio.ObtenerTodas();
        }

        public async Task<Tarea?> ObtenerPorId(int id)
        {
            return await _tareaRepositorio.ObtenerPorId(id);
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorEstatus(int estatus)
        {
            return await _tareaRepositorio.ObtenerPorEstatus(estatus);
        }

        public async Task<Tarea> CrearTarea(Tarea tarea)
        {
            var newId = await _tareaRepositorio.CrearTarea(tarea);
            tarea.Id = newId;
            return tarea;
        }

        public async Task<bool> EliminarTarea(int id)
        {
            return await _tareaRepositorio.EliminarTarea(id);
        }

        
    }
}
