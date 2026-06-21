using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PEQUENADESPESA", Nome = "PrestacaoContas.PEQUENADESPESA")]
    public class PequenaDespesa : IEntity
    {
        [AtributoCampo(Nome = "PEQUENADESPESAID")]
        public int PequenaDespesaId { get; set; }

        [AtributoCampo(Nome = "EVENTOID")]
        public int EventoId { get; set; }

        [AtributoCampo(Nome = "TIPOTRANSPORTEID")]
        public int TipoTransporteId { get; set; }

        [AtributoCampo(Nome = "TIPODESPESA")]
        public string TipoDespesa { get; set; }

        [AtributoCampo(Nome = "FORMAPAGAMENTO")]
        public string FormaPagamento { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "ORIGEM")]
        public string Origem { get; set; }

        [AtributoCampo(Nome = "DESTINO")]
        public string Destino { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}