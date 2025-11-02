using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAPI.Exceptions;
using TestAPI.Modelos;
using TestAPI.Servicios;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TareaController : ControllerBase

    {
        //Servicio
        private readonly ITareaServicio _tareaServicio;

        //Constructor que da valor inicial a los servicios y/o propiedades
        public TareaController(ITareaServicio tareaServicio)
        {
            _tareaServicio = tareaServicio;
        }

        //Definición de endpoints
        //Obtener todas las tareas
        [HttpGet]
   
        
        public async Task<IEnumerable<Tarea>> Obtener()
        {
           var lista = await _tareaServicio.ObtenerTodas();
            if (lista == null || !lista.Any())
                throw new NoDataException("No hay tareas disponibles");
            return lista;
        }

        //Obtener tarea por id
        [HttpGet("{id:int}", Name = "ObtenerTareaPorId")]
        [Authorize]

        public async Task<ActionResult<Tarea>> ObtenerPorId(int id)
        {
            var tarea = await _tareaServicio.ObtenerPorId(id);
            if (tarea == null)
                throw new NoDataException("no hay tareas con ese id");
            return Ok(tarea);
        }

        //Obtener tarea por estatus
        [HttpGet("estatus/{estatus:int}")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<Tarea>>> ObtenerPorEstatus(int estatus)
        {
            var tareas = await _tareaServicio.ObtenerPorEstatus(estatus);
            if (tareas == null || !tareas.Any())
                throw new NoDataException("no hay tareas con ese estatus");
            return Ok(tareas);
        }

        //Creación de una tarea
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<Tarea>> Crear([FromBody] Tarea nuevaTarea)
        {
            if (nuevaTarea == null) return BadRequest();
            var creada = await _tareaServicio.CrearTarea(nuevaTarea);

            nuevaTarea.Id = 0;

            
            return CreatedAtRoute("ObtenerTareaPorId", new { id = creada.Id }, creada);
        }

        //Eliminar una tarea
        [HttpDelete("{id:int}")]
        [Authorize]

        public async Task<IActionResult> Eliminar(int id)
        {
            var tareas = await _tareaServicio.EliminarTarea(id);
            if (tareas == null)
                throw new NoDataException("no hay tareas con ese id");
            return Ok(tareas);
        }



    }
}
