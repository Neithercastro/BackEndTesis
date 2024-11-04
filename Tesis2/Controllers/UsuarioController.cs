using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Tesis2.DTO;
using Tesis2.Entities;

namespace Tesis2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly EstilosaprendizajeContext _context;
        private IConfiguration _config;

        public UsuarioController(EstilosaprendizajeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpPost("Validar")]
        public async Task<ActionResult> ValidarUsuario(CredencialesDTO Object)
        {
            Usuario Usuario = await _context.Usuarios.Select(
                Data => new Usuario
                {
                    Nombre = Data.Nombre,
                    CorreoElectronico = Data.CorreoElectronico,
                    Usuario1 = Data.Usuario1,
                    Contraseña = Data.Contraseña,
                    Idestilos = Data.Idestilos,
                    Semestres= Data.Semestres
                    
                }
            ).FirstAsync(Request => Request.Usuario1.Equals(Object.Usuario));

            if (Usuario.Contraseña != Object.Contrasena)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            else if (Usuario.Idestilos == 5)
            {
                object Problema = JsonConvert.SerializeObject("Sin confirmar");

                return Accepted(Problema);
            }
            else
            {
                var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

                var TokenData = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: Credentials);

                var Token = new JwtSecurityTokenHandler().WriteToken(TokenData);

                string SessionToken = JsonConvert.SerializeObject(Token);

                return Ok(SessionToken);
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostUsuario(UsuarioDTO Object)
        {
            var Data = new Usuario()
            {
                Nombre = Object.Nombre,
                Usuario1 = Object.usuario,
                CorreoElectronico = Object.correo,
                Contraseña = Object.contrasena,
                Idestilos  = Object.idestilos,
                Semestres = Object.semestre
            };
            _context.Usuarios.Add(Data);
            await _context.SaveChangesAsync();
            return Ok(Data);
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarios(string UsuarioUsuarios)
        {
            UsuarioDTO Usuario = await _context.Usuarios
                .Where(Data => Data.Usuario1 == UsuarioUsuarios)
                .Select(
                  Data => new UsuarioDTO
                  {
                      Id = Data.Idusuarios,
                      Nombre = Data.Nombre,
                      usuario = Data.Usuario1,
                      correo = Data.CorreoElectronico,
                      contrasena = Data.Contraseña,
                      idestilos = Data.Idestilos,
                      NombreEstilo = Data.IdestilosNavigation != null? Data.IdestilosNavigation.Nombre : null,
                      semestre = Data.Semestres
                                         }
            ).FirstAsync(Request => Request.usuario.Equals(UsuarioUsuarios));

            if (Usuario == null)
            {
                return NotFound();
            }
            else
            {
                return Usuario;
            }
        }


    }
}
