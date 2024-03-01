using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("ContatoEmail")]
    public class ContatoEmail
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string assunto { get; set; }
        public string mensagem { get; set; }
        public DateTime data { get; set; }
    }
}
