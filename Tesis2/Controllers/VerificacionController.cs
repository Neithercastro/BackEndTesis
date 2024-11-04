using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Tesis2.DTO;
using Tesis2.Entities;

namespace Tesis2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificacionController : ControllerBase
    {
        private readonly EstilosaprendizajeContext _context;
        private IConfiguration _config;

        public VerificacionController(EstilosaprendizajeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpPost("Registrar")]
        public async Task<ActionResult> PostVerificacion(VerificacionDTO Object)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Email"] + Object.Usuario));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var TokenData = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: Credentials);

            var Token = new JwtSecurityTokenHandler().WriteToken(TokenData);

            var Data = new Verificacion()
            {
                Usuario = Object.Usuario,
                Permiso = Object.Permiso,
                Token = Token
            };

            string EmailToken = JsonConvert.SerializeObject(Token);

            _context.Verificacions.Add(Data);
            await _context.SaveChangesAsync();

            return Ok(EmailToken);
        }

    }
       
}
