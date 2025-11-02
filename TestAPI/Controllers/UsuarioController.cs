using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestAPI.Exceptions;
using TestAPI.Modelos;
using TestAPI.Servicios;
using System.Collections.Generic;
using ZstdSharp;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServicio _usuarioService;
        private readonly IConfiguration _configuration;

        public UsuarioController(IUsuarioServicio usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        // GET /usuario
        [HttpGet]
        [Authorize]

        public async Task<ActionResult<List<Usuarios>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerTodas();

            return Ok(usuarios);
        }

        // GET /usuario/(id)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Usuarios>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // POST /usuario
        [HttpPost]
        [Authorize]

        public async Task<ActionResult> CrearUsuario([FromBody] Usuarios usuario)
        {
            var creado = await _usuarioService.CrearUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = creado.Id }, creado);
        }

        // DELETE /usuario/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var eliminado = await _usuarioService.EliminarUsuario(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }

        // POST/usuario/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] Usuarios login)
        {

            var usuarios = await _usuarioService.ObtenerTodas();
            var usuario = usuarios.FirstOrDefault(u =>
                u.NombreUsuario.Trim().ToLower() == login.NombreUsuario.Trim().ToLower() &&
                u.Contraseña == login.Contraseña);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
            }
        

    // generacion de token
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds
    );

    return Ok(new
    {
        mensaje = "Login exitoso",
        token = new JwtSecurityTokenHandler().WriteToken(token)
    });
}
    }
}