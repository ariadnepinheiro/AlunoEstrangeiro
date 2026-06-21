using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.APLICACAOFINANCEIRA", Nome = "PrestacaoContas.APLICACAOFINANCEIRA")]
    public class AplicacaoFinanceira : IEntity
    {
        [AtributoCampo(Nome = "APLICACAOFINANCEIRAID")]
        public int AplicacaoFinanceiraId { get; set; }

        public string Censo { get; set; }

        [AtributoCampo(Nome = "EXTRATOBANCARIOID")]
        public int ExtratoBancarioId { get; set; }

        [AtributoCampo(Nome = "VALOR")]
        public decimal Valor { get; set; }

        [AtributoCampo(Nome = "DATALANCAMENTO")]
        public DateTime DataLancamento { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
