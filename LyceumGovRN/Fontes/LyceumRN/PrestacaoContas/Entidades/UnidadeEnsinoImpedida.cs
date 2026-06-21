using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.UNIDADEENSINOIMPEDIDA", Nome = "PrestacaoContas.UNIDADEENSINOIMPEDIDA")]
    public class UnidadeEnsinoImpedida : IEntity
    {
        [AtributoCampo(Nome = "UNIDADEENSINOIMPEDIDAID")]
        public int UnidadeEnsinoImpedidaId { get; set; }

        [AtributoCampo(Nome = "MOTIVOIMPEDIMENTOID")]
        public int MotivoImpedimentoId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
