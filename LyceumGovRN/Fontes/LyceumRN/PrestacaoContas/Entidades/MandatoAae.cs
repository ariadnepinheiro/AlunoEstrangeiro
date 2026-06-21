using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.MANDATOAAE", Nome = "PrestacaoContas.MANDATOAAE")]
    public class MandatoAae : IEntity
    {       
        [AtributoCampo(Nome = "MANDATOAAEID")]
        public int MandatoAaeId { get; set; }

        [AtributoCampo(Nome = "TESOUREIROID")]
        public int? TesoureiroId { get; set; }

        [AtributoCampo(Nome = "PESSOATESOUREIRO")]
        public decimal? PessoaTesoureiro { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "MANDATO")]
        public int Mandato { get; set; }

        [AtributoCampo(Nome = "DATAINICIOMANDATO")]
        public DateTime DataInicioMandato { get; set; }

        [AtributoCampo(Nome = "DATAFIMMANDATO")]
        public DateTime DataFimMandato { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

