using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public EstiloAprendizajeController(EstilosaprendizajeContext context)
        {
            _context = context;
        }

        // POST api/<EstiloAprendizajeController>
        [HttpPost]
        public async Task<IActionResult> ProcesarRespuestas([FromBody] RespuestasDTO respuestasDto)
        {
            if (respuestasDto.Respuestas.Length < 16)
            {
                return BadRequest("Se requieren 16 respuestas como minimo.");
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

            usuario.Idestilos = estiloAprendizajeId.Value;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estilo de aprendizaje registrado con éxito." });
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

