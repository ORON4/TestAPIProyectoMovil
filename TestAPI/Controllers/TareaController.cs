using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAPI.Exceptions;
using TestAPI.Modelos;
using TestAPI.Repositorios;
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

        public async Task<ActionResult<IEnumerable<Tarea>>> Obtener()
        {
            var lista = await _tareaServicio.ObtenerTodas();
            return Ok(lista);
        }

        //Obtener tarea por id
        [HttpGet("{id:int}", Name = "ObtenerTareaPorId")]
        

        public async Task<ActionResult<Tarea>> ObtenerPorId(int id)
        {
            var tarea = await _tareaServicio.ObtenerPorId(id);
            if (tarea == null)
                throw new NoDataException("no hay tareas con ese id");
            return Ok(tarea);
        }

        //Obtener tarea por estatus
        [HttpGet("estatus/{estatus:int}")]
        

        public async Task<ActionResult<IEnumerable<Tarea>>> ObtenerPorEstatus(int estatus)
        {
            var tareas = await _tareaServicio.ObtenerPorEstatus(estatus);
            if (tareas == null || !tareas.Any())
                throw new NoDataException("no hay tareas con ese estatus");
            return Ok(tareas);
        }

        //Creación de una tarea
        [HttpPost]
        

        public async Task<ActionResult<Tarea>> Crear([FromBody] Tarea nuevaTarea)
        {
            if (nuevaTarea == null) return BadRequest();
            var creada = await _tareaServicio.CrearTarea(nuevaTarea);

            nuevaTarea.Id = 0;

            
            return CreatedAtRoute("ObtenerTareaPorId", new { id = creada.Id }, creada);
        }

        //Eliminar una tarea
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var tareas = await _tareaServicio.EliminarTarea(id);
            if (tareas == null)
                throw new NoDataException("no hay tareas con ese id");
            return Ok(tareas);
        }


        //Actualizar una tarea
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarTarea(int id, Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo");
            }

            var resultado = await _tareaServicio.ActualizarTarea(tarea);

            if (resultado)
            {
                return NoContent(); // Éxito (Código 204)
            }

            return NotFound(); // O un error
        }



    }
}
