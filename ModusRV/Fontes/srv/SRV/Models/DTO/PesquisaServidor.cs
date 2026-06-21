using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class PesquisaServidor
    {
        [Display(Name = "Matrícula")]
        public int? Matricula { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        public Paging<Servidor> PageServidores { get; set; }

        /// <summary>
        /// Regional selecionada
        /// </summary>
        public int? IdRegionalDefault { get; set; }

        /// <summary>
        /// Unidade Administrativa selecionada
        /// </summary>
        public int? IdUnidadeAdministrativaDefault { get; set; }

        public int? IdAnoReferencia { get; set; }
    }
}