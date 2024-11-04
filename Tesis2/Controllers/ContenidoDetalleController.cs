using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesis2.DTO;
using Tesis2.Entities;

namespace Tesis2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContenidoDetalleController : ControllerBase
    {
        private readonly EstilosaprendizajeContext _context;

        public ContenidoDetalleController(EstilosaprendizajeContext context)
        {
            _context = context;
        }
        [HttpGet("{idActividad}/{idEstiloAprendizaje}")]
        public async Task<IActionResult> GetContenidoDetalle(int idActividad, int idEstiloAprendizaje)
        {
            var contenidoDetalle = await _context.Contenidodetalles
                .Where(cd => cd.IdActividad == idActividad && cd.IdEstiloAprendizaje == idEstiloAprendizaje)
                .Select(cd => new ContenidodetalleDTO
                {
                    IdContenido = cd.IdContenido,
                    IdMateria = cd.IdMateria,
                    IdActividad = cd.IdActividad,
                    IdEstiloAprendizaje = cd.IdEstiloAprendizaje,
                    Descripcion = cd.Descripcion,
                    MaterialApoyo1 = cd.MaterialApoyo1,
                    MaterialApoyo2 = cd.MaterialApoyo2,
                    NombreEstiloAprendizaje = cd.IdEstiloAprendizajeNavigation.Nombre,
                    NombreMateria = cd.IdMateriaNavigation.Nombre,
                    nombreActividad = cd.IdMateriaDetalleNavigation.NombreActividad
                })
                .ToListAsync();

            if (!contenidoDetalle.Any())
            {
                return NotFound();
            }

            return Ok(contenidoDetalle);
        }
    }
}
