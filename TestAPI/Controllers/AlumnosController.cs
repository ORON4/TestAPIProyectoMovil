using Microsoft.AspNetCore.Mvc;
using TestAPI.Modelos;
using TestAPI.Servicios;
using System.Collections.Generic;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly IAlumnosServicio _alumnosServicio;

        public AlumnosController(IAlumnosServicio alumnosServicio)
        {
            _alumnosServicio = alumnosServicio;
        }

        [HttpGet("grupo/{idGrupo:int}")] // Ruta: GET Alumnos/grupo/1
        public async Task<ActionResult<IEnumerable<Alumno>>> ObtenerPorGrupo(int idGrupo)
        {
            var alumnos = await _alumnosServicio.ObtenerPorGrupo(idGrupo);
            if (alumnos == null || !alumnos.Any())
            {
                return NotFound("No se encontraron alumnos para ese grupo.");
            }
            return Ok(alumnos);
        }

        [HttpGet("{id:int}")] // Ruta: GET Alumno/5
        public async Task<ActionResult<Alumno>> ObtenerPorId(int id)
        {
            var alumno = await _alumnosServicio.ObtenerPorId(id);
            if (alumno == null)
            {
                return NotFound("No se encontró ningún alumno con ese ID.");
            }
            return Ok(alumno);
        }
    }
}
