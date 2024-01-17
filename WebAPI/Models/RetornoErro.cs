using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RetornoErro
    {
        public RetornoErro()
        {
            Erros = new List<string>();
        }

        public bool Sucesso { get; set; }
        public List<string> Erros { get; set; }
    }
}
