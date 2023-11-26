using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        public int Id { get; private set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordStamp { get; set; }
        
    }
}
