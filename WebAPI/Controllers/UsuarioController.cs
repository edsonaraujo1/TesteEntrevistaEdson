using Authentiction;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepositories;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepository usuarioRepositories, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepositories = usuarioRepositories;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuario = await _usuarioRepositories.SelecionarTodos();
            if (usuario == null)
            {
                return NotFound("Nenhum Registro não encontrdo!");
            }

            return Ok(usuario);
        }
        [HttpPost("Register")]
        public async Task<ActionResult> CadastrarUsuario(Usuario usuario)
        {
            var ret = await _usuarioRepositories.UserExists(usuario.Email);
            if (ret != null)
            {
                return NotFound("Email já Cadastrado!");
            }

            var authentiction = new AutenticarJWT(null, null, null, usuario.PasswordHash.ToString());
            var resultado = authentiction.Criptografar();

            usuario.PasswordHash = resultado;
            usuario.PasswordStamp = "string";

            try
            {
                _usuarioRepositories.Incluir(usuario);
                if (await _usuarioRepositories.SaveAllAsync())
                {
                    return Ok(new
                    {
                        StatusCode = "OK",
                        Mensage = "Registro Salvo com sucesso!"

                    });
                    
                }

            }
            catch (System.Exception e)
            {

                throw e;
            }
            return BadRequest("Ocorreu um erro ao salvar registro.");
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> AlterarUsuario(Usuario usuario)
        {
            var ret = await _usuarioRepositories.UserExists(usuario.Email);
            if (ret == null)
            {
                return NotFound("Email Invalido!!");
            }

            var authentiction = new AutenticarJWT(null, null, null, usuario.PasswordHash.ToString());
            var resultado = authentiction.Criptografar();

            usuario.PasswordHash = resultado;

            _usuarioRepositories.Alterar(usuario);
            if (await _usuarioRepositories.SaveAllAsync())
            {
                return Ok("Registro Alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar registro.");
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _usuarioRepositories.SelecionarByPk(id);
            if (usuario == null)
            {
                return NotFound("Registro não encontrdo!");
            }
            _usuarioRepositories.Excluir(usuario);
            if (await _usuarioRepositories.SaveAllAsync())
            {
                return Ok("Registro Excluido com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao excluir o registro.");
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarUsuario(int id)
        {
            var usuario = await _usuarioRepositories.SelecionarByPk(id);
            if (usuario == null)
            {
                return NotFound("Registro não encontrdo!");
            }

            var authentiction = new AutenticarJWT(null, null, null, usuario.PasswordHash.ToString());
            var resultado = authentiction.Descriptografar();

            usuario.PasswordHash = resultado;

            UsuarioDTO usuarioDTO = new UsuarioDTO
            {
                Id = usuario.Id,
                NomeUsuario = usuario.NomeUsuario,
                Email = usuario.Email
            };

            return Ok(usuario);
        }

    }
}
