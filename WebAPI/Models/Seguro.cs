﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("Seguro")]
    public class Seguro
    {
        [DisplayName("Codigo")]
        public int id { get; set; }
        [Required]
        [DisplayName("Nome Cliente")]
        public string Cliente { get; set; }
        [Required]
        [DisplayName("CPF Cliente")]
        [StringLength(14)]
        public string CPF { get; set; }
        [DisplayName("Idade")]
        public int Idade { get; set; }
        [DisplayName("Veiculo")]
        public string Veiculo { get; set; }
        [DisplayName("Marca")]
        public string Marca { get; set; }
        [DisplayName("Modelo")]
        public string Modelo { get; set; }
        [Required]
        [DisplayName("Valor do Veiculo")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorVeiculo { get; set; }
        [DisplayName("Valor do Seguro")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorSeguro { get; set; }
    }
}
