using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    [AtributoTabela("Patrimonio.TRANSFERENCIAITEM", Nome = "Patrimonio.TRANSFERENCIAITEM")]
    public class TransferenciaItem : IEntity
    {
        [AtributoCampo(Nome = "TRANSFERENCIAITEMID")]
        public int TransferenciaItemId { get; set; }

        [AtributoCampo(Nome = "TRANSFERENCIAID")]
        public int TransferenciaId { get; set; }

        [AtributoCampo(Nome = "NUMEROBEMORIGEM")]
        public int NumeroBemOrigem { get; set; }

        [AtributoCampo(Nome = "BEMID")]
        public int BemId { get; set; }

        public string Situacao { get; set; }

        public string Justificativa { get; set; }

        public decimal? Valor { get; set; }

        [AtributoCampo(Nome = "MOEDAID")]
        public int? MoedaId { get; set; }
    }
}