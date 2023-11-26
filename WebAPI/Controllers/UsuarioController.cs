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
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepositories;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepository usuarioRepositories, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepositories = usuarioRepositories;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _usuarioRepositories.SelecionarTodos());
        }
        [HttpPost("Register")]
        public async Task<ActionResult> CadastrarUsuario(Usuario usuario)
        {
            var ret = await _usuarioRepositories.UserExists(usuario.Email);
            if (ret != null)
            {
                return NotFound("Email já Cadastrado!");
            }

            _usuarioRepositories.Incluir(usuario);
            if (await _usuarioRepositories.SaveAllAsync())
            {
                return Ok("Registro Salvo com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao salvar registro.");
        }
        [HttpPut]
        public async Task<ActionResult> AlterarUsuario(Usuario usuario)
        {
            var ret = await _usuarioRepositories.UserExists(usuario.Email);
            if (ret != null)
            {
                return NotFound("Email Invalido!!");
            }
            _usuarioRepositories.Alterar(usuario);
            if (await _usuarioRepositories.SaveAllAsync())
            {
                return Ok("Registro Alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar registro.");
        }
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
        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarUsuario(int id)
        {
            var usuario = await _usuarioRepositories.SelecionarByPk(id);
            if (usuario == null)
            {
                return NotFound("Registro não encontrdo!");
            }
            UsuarioDTO usuarioDTO = new UsuarioDTO
            {
                Id = usuario.Id,
                NomeUsuario = usuario.NomeUsuario,
                Email = usuario.Email
            };

            return Ok(usuarioDTO);
        }

    }
}
