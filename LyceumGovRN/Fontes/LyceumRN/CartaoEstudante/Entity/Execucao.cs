using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class Execucao : IEntity
    {
        [AtributoCampo(Nome = "EXECUCAOID")]
        public int ExecucaoId { get; set; }

        [AtributoCampo(Nome = "PROCESSOID")]
        public int ProcessoId { get; set; }

        [AtributoCampo(Nome = "DATAEXECUCAOINICIO")]
        public DateTime DataInicioExecucao { get; set; }

        [AtributoCampo(Nome = "DATAEXECUCAOFIM")]
        public DateTime DataFimExecucao { get; set; }

        [AtributoCampo(Nome = "SITUACAOEXECUCAO")]
        public bool SituacaoExecucao { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

    }
}
