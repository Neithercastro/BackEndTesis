using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tesis2.DTO;
using Tesis2.Entities;

namespace Tesis2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly EstilosaprendizajeContext _context;

        public MateriasController(EstilosaprendizajeContext context)
        {
            _context = context;
        }

        [HttpGet("semestre")]
        public async Task<ActionResult<List<Materia>>> GetMateriasPorSemestre(int semestre)
        {
            // Consulta solo las materias que coincidan con el semestre
            var materias = await _context.Materias
                .Where(m => m.Semestre == semestre)
                .Select(m => new MateriasDTO
                {
                    Id = m.IdMaterias,
                    Nombre = m.Nombre,
                    semestre = m.Semestre,
                    ImagenUrl = m.DireccionImagen
                })
                .ToListAsync();

            // Verifica si se encontraron materias
            if (materias == null || !materias.Any())
            {
                return NotFound("No se encontraron materias para el semestre especificado.");
            }

            return Ok(materias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMateriaDetalle(int id)
        {
            var materiaDetalle = await _context.Materiadetalles
                .Include(md => md.IdMateriaNavigation)
                .Where(md => md.IdMateria == id)
                .ToListAsync();

            if (materiaDetalle == null)
            {
                return NotFound();
            }

            var result = materiaDetalle.Select(materiaDetalle => new MateriaDetalleDTO
            {
                Id = materiaDetalle.IdMateriaDetalle,
                idmateria = materiaDetalle.IdMateria,
                nombreactivdad = materiaDetalle.NombreActividad,
                nombremateria = materiaDetalle.IdMateriaNavigation?.Nombre 
            }).ToList();

            return Ok(result);
        }
    }
}
