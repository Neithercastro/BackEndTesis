using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Tesis2.DTO;
using Tesis2.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tesis2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstiloAprendizajeController : ControllerBase
    {
        private readonly EstilosaprendizajeContext _context;
        private IConfiguration _config;
        public EstiloAprendizajeController(EstilosaprendizajeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST api/<EstiloAprendizajeController>
        // POST api/<EstiloAprendizajeController>
        [HttpPost]
        public async Task<IActionResult> ProcesarRespuestas([FromBody] RespuestasDTO respuestasDto)
        {
            if (respuestasDto.Respuestas.Length < 16)
            {
                return BadRequest("Se requieren 16 respuestas como mínimo.");
            }

            // Procesar respuestas y determinar el estilo de aprendizaje
            var estiloAprendizajeId = DeterminarEstiloAprendizaje(respuestasDto.Respuestas);

            if (estiloAprendizajeId == null)
            {
                return BadRequest("No se pudo determinar el estilo de aprendizaje.");
            }

            // Obtener el usuario y actualizar su estilo de aprendizaje
            var usuario = await _context.Usuarios.FindAsync(respuestasDto.UsuarioId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar el estilo de aprendizaje
            usuario.Idestilos = estiloAprendizajeId.Value;
            await _context.SaveChangesAsync();

            // Generar el token JWT
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenData = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenData);

            // Obtener el nombre del estilo actualizado
            var estiloAprendizaje = await _context.Estilosaprendizajes
                .Where(e => e.IdestilosAprendizaje == usuario.Idestilos)
                .FirstOrDefaultAsync();

            // Devolver el token y el estilo de aprendizaje actualizado
            return Ok(new
            {
                token = token,
                estilo = estiloAprendizaje?.Nombre
            });
        }


        private int? DeterminarEstiloAprendizaje(string[] respuestas)
        {
            // Lógica para determinar el estilo de aprendizaje basado en las respuestas
            // Por ejemplo, podrías contar las ocurrencias de cada tipo de respuesta
            var contadorRespuestas = new Dictionary<string, int>();

            foreach (var respuesta in respuestas)
            {
                if (contadorRespuestas.ContainsKey(respuesta))
                {
                    contadorRespuestas[respuesta]++;
                }
                else
                {
                    contadorRespuestas[respuesta] = 1;
                }
            }

            // Determinar el estilo de aprendizaje más común
            var estiloAprendizajeNombre = contadorRespuestas.OrderByDescending(c => c.Value).First().Key;

            // Buscar el ID del estilo de aprendizaje basado en su nombre
            var estiloAprendizaje = _context.Estilosaprendizajes.FirstOrDefault(e => e.Nombre == estiloAprendizajeNombre);

            return estiloAprendizaje?.IdestilosAprendizaje;
        }
    }

}

