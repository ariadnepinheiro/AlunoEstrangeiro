using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroServidor
    {
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        public Paging<Servidor> Servidores { get; set; }
    }
}