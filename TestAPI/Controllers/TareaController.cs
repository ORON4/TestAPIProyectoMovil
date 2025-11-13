using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly ITareaServicio _tareaServicio;

        public TareaController(ITareaServicio tareaServicio)
        {
            _tareaServicio = tareaServicio;
        }

        // 1. OBTENER TAREAS DE UN ALUMNO
        // GET: /tarea/alumno/5
        [HttpGet("alumno/{alumnoId:int}")]
        public async Task<ActionResult<IEnumerable<TareaAlumnoDTO>>> ObtenerPorAlumno(int alumnoId)
        {
            var lista = await _tareaServicio.ObtenerPorAlumno(alumnoId);
            // Si la lista está vacía, devolvemos una lista vacía en lugar de error,
            // para que el frontend simplemente muestre "No hay tareas".
            return Ok(lista);
        }

        // 2. CREAR UNA TAREA (ASIGNADA A TODOS)
        // POST: /tarea
        [HttpPost]
        public async Task<ActionResult<Tarea>> Crear([FromBody] Tarea nuevaTarea)
        {
            if (nuevaTarea == null) return BadRequest("La tarea no puede ser nula");

            // Esto crea la tarea global y la inserta en la tabla de todos los alumnos
            var creada = await _tareaServicio.CrearTareaParaTodos(nuevaTarea);

            return Ok(creada);
        }

        // 3. MARCAR TAREA COMO ENTREGADA
        // PUT: /tarea/entregar/105 (Donde 105 es el AlumnoTareaId, NO el Id global)
        [HttpPut("entregar/{alumnoTareaId:int}")]
        public async Task<IActionResult> MarcarEntregada(int alumnoTareaId)
        {
            var exito = await _tareaServicio.MarcarEntregada(alumnoTareaId);

            if (!exito)
                return NotFound("No se encontró la asignación de tarea para actualizar.");

            return NoContent(); // 204 No Content (Éxito)
        }

        // 4. ELIMINAR TAREA DE LA VISTA DE UN ALUMNO
        // DELETE: /tarea/asignacion/105 (Donde 105 es el AlumnoTareaId)
        [HttpDelete("asignacion/{alumnoTareaId:int}")]
        public async Task<IActionResult> EliminarDeAlumno(int alumnoTareaId)
        {
            var exito = await _tareaServicio.EliminarTareaDeAlumno(alumnoTareaId);

            if (!exito)
                return NotFound("No se encontró la asignación de tarea para eliminar.");

            return NoContent();
        }

        // 5. ACTUALIZAR DEFINICIÓN GLOBAL (Título, Descripción, etc.)
        // PUT: /tarea/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarDefinicion(int id, [FromBody] Tarea tarea)
        {
            if (id != tarea.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo");

            var exito = await _tareaServicio.ActualizarDefinicion(tarea);

            if (!exito)
                return NotFound("No se encontró la tarea original.");

            return NoContent();
        }

        // (Opcional) Obtener definición global por ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tarea>> ObtenerDefinicion(int id)
        {
            var tarea = await _tareaServicio.ObtenerDefinicion(id);
            if (tarea == null) return NotFound();
            return Ok(tarea);
        }
    }
}