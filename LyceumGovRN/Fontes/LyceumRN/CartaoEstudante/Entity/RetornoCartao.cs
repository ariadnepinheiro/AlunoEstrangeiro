using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class RetornoCartao: IEntity
    {
        [AtributoCampo(Nome = "RETORNOCARTAOID")]
        public int RetornoCartaoId { get; set; }

        [AtributoCampo(Nome = "TIPOSITUACAOCARTAOID")]
        public int TipoSituacaoCartaoId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraID { get; set; }

        [AtributoCampo(Nome = "TIPOCANCELAMENTOID")]
        public int? TipoCancelamentoId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "NUMEROCHIP")]
        public string NumeroChip { get; set; }

        [AtributoCampo(Nome = "NUMEROCARTAO")]
        public string NumeroCartao { get; set; }

        [AtributoCampo(Nome = "NUMEROLOTE")]
        public int? NumeroLote { get; set; }

        [AtributoCampo(Nome = "DATAIMPRESSAO")]
        public DateTime? DataImpressao { get; set; }

        [AtributoCampo(Nome = "DATAUTILIZACAO")]
        public DateTime? DataUtilizacao { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [AtributoCampo(Nome = "IDBENEFICIARIO")]
        public int? IdBeneficiario { get; set; }

        [AtributoCampo(Nome = "LOCALIMPRESSAO")]
        public string LocalImpressao { get; set; }

        [AtributoCampo(Nome = "DATAENTREGALOTE")]
        public DateTime? DataEntregaLote { get; set; }

        [AtributoCampo(Nome = "DATACONFIRMACAOENTREGA")]
        public DateTime? DataConfirmacaoEntrega { get; set; }
    }
}
