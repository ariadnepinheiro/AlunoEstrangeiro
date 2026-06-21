using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    [AtributoTabela("PrestacaoContas.PERIODOCONFIRMACAO", Nome = "PrestacaoContas.PERIODOCONFIRMACAO")]
    public class PeriodoConfirmacao : IEntity
    {
        [AtributoCampo(Nome = "PERIODOCONFIRMACAOID")]
        public int PeriodoConfirmacaoId { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
