using Authentiction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        public readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepositories;
        public AuthController(IConfiguration configuration, IUsuarioRepository usuarioRepositories)
        {
            _configuration = configuration;
            _usuarioRepositories = usuarioRepositories;
        }

        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] Usuario request)
        {
            var authentiction = new AutenticarJWT(null, null, null, request.PasswordHash);
            var resultado = authentiction.Criptografar();

            var ret = await _usuarioRepositories.SelecionarEmail(request.Email, resultado);
            if (ret == null)
            {
                return NotFound("Usuário ou Senha Invalidos.!!!");
            }

            if (ret != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Email)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

                var token = new JwtSecurityToken(
                    issuer: "utyum.com.br",
                    audience: "utyum.com.br",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    nome = ret.NomeUsuario,
                    user = ret.Email,
                    expira = DateTime.Now.AddMinutes(30)
                });
            }
            return BadRequest("Usuário ou Senha Incorretas!!");
        }
    }
}
