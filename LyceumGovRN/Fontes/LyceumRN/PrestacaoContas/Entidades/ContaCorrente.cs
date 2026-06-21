using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.CONTACORRENTE", Nome = "PrestacaoContas.CONTACORRENTE")]
    public class ContaCorrente : IEntity
    {
        [AtributoCampo(Nome = "CONTACORRENTEID")]
        public int ContaCorrenteId { get; set; }

        [AtributoCampo(Nome = "REGIONALID")]
        public int? RegionalId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "BANCO")]
        public string Banco { get; set; }

        [AtributoCampo(Nome = "AGENCIA")]
        public string Agencia { get; set; }

        [AtributoCampo(Nome = "CONTA")]
        public string Conta { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime? DataInicio { get; set; }

        public String DataInicioValida { get; set; }

        public bool DataInicioInvalida { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        public String DataFimValida { get; set; }

        public bool DataFimInvalida { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
