using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.SALDOINICIAL", Nome = "PrestacaoContas.REGIAOFGV")]
    public class SaldoInicial : IEntity
    {
        [AtributoCampo(Nome = "SALDOINICIALID")]
        public int SaldoInicialId { get; set; }

        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "VALORINICIAL")]
        public decimal ValorInicial { get; set; }

        [AtributoCampo(Nome = "DATAREFERENCIACALCULO")]
        public DateTime DataReferenciaCalculo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        public bool DataInvalida { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
