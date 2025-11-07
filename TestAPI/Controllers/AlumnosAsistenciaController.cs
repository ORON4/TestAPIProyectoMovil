using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAPI.Modelos;
using TestAPI.Servicios;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")] // Ruta base /AlumnosAsistencia
    public class AlumnosAsistenciaController : ControllerBase
    {
        private readonly IAlumnosAsistenciaServicio _asistenciaServicio;

        public AlumnosAsistenciaController(IAlumnosAsistenciaServicio asistenciaServicio)
        {
            _asistenciaServicio = asistenciaServicio;
        }

        // --- MODIFICADO / NUEVO ENDPOINT 'POST' ---
        [HttpPost]
        public async Task<IActionResult> GuardarAsistencia([FromBody] AlumnosAsistencia asistencia)
        {
            if (asistencia == null)
                return BadRequest();

            var resultado = await _asistenciaServicio.GuardarAsistencia(asistencia);

            if (resultado)
            {
                return Ok(); // O return NoContent();
            }

            return BadRequest("No se pudo guardar la asistencia.");
        }

        [HttpGet("fecha/{fecha:datetime}")]
        public async Task<ActionResult<IEnumerable<AsistenciaReporte>>> ObtenerPorFecha(DateTime fecha)
        {
            var reporte = await _asistenciaServicio.ObtenerPorFecha(fecha);

            if (reporte == null || !reporte.Any())
            {
                return NotFound("No se encontraron asistencias para esa fecha.");
            }

            return Ok(reporte);
        }
    }
}