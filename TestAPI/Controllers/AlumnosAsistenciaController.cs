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
    public class AlumnosAsistenciaController : ControllerBase

    {
        //Servicio
        private readonly IAlumnosAsistenciaServicio _AlumnosAsistenciaServicio;

        //Constructor que da valor inicial a los servicios y/o propiedades
        public AlumnosAsistenciaController(IAlumnosAsistenciaServicio alumnosAsistenciaServicio)
        {
            _AlumnosAsistenciaServicio = alumnosAsistenciaServicio;
        }

        //Definición de endpoints
        //Obtener todas las asistencias
        [HttpGet]

        public async Task<IEnumerable<AlumnosAsistencia>> Obtener()
        {
            var lista = await _AlumnosAsistenciaServicio.ObtenerTodas();
            if (lista == null || !lista.Any())
                throw new NoDataException("No hay datos disponibles");
            return lista;
        }

        //Creación de una asistencia
        [HttpPost]
        public async Task<ActionResult<AlumnosAsistencia>>Crear([FromBody] AlumnosAsistencia nuevaAlumnosAsistencia)
        {
            if (nuevaAlumnosAsistencia == null) return BadRequest();
            var creada = await _AlumnosAsistenciaServicio.CrearAlumnosAsistencia(nuevaAlumnosAsistencia);

            nuevaAlumnosAsistencia.Id = 0;


            return CreatedAtRoute("ObtenerTareaPorId", new { id = creada.Id }, creada);
        }

        //Eliminar una asistencia
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var alumnosAsistencia = await _AlumnosAsistenciaServicio.EliminarAlumnosAsistencia(id);
            if (alumnosAsistencia == null)
                throw new NoDataException("no hay asistencias con ese id");
            return Ok(alumnosAsistencia);
        }








    }
}

