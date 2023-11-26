using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTOs
{
    [Table("Usuario")]
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }

    }
}
