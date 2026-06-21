using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.LANCAMENTOREPASSE", Nome = "PrestacaoContas.LANCAMENTOREPASSE")]
    public class LancamentoRepasse : IEntity
    {
        [AtributoCampo(Nome = "LANCAMENTOREPASSEID")]
        public int LancamentoRepasseId { get; set; }

        [AtributoCampo(Nome = "WSREPASSESEFAZID")]
        public int? WsRepasseSefazId { get; set; }

        [AtributoCampo(Nome = "ITEMPLANILHAORCAMENTARIAID")]
        public int ItemPlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "CONTACORRENTEID")]
        public int ContaCorrenteId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "VALOR")]
        public decimal Valor { get; set; }

        [AtributoCampo(Nome = "VALORPAGO")]
        public decimal ValorPago { get; set; }

        [AtributoCampo(Nome = "NUMERONE")]
        public string NumeroNe { get; set; }

        [AtributoCampo(Nome = "NUMERONL")]
        public string NumeroNl { get; set; }

        [AtributoCampo(Nome = "NUMEROOB")]
        public string NumeroOb { get; set; }

        [AtributoCampo(Nome = "NUMEROPD")]
        public string NumeroPd { get; set; }

        [AtributoCampo(Nome = "NUMEROLISTAOB")]
        public string NumeroListaOb { get; set; }

        [AtributoCampo(Nome = "STATUSLISTA")]
        public string StatusLista { get; set; }

        [AtributoCampo(Nome = "NUMEROPROCESSOREPASSE")]
        public string NumeroProcessoRepasse { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
