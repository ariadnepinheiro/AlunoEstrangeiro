using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class Processo : IEntity
    {
        [AtributoCampo(Nome = "PROCESSOID")]
        public int ProcessoId { get; private set; }

        [AtributoCampo(Nome = "NOMEPROCESSO")]
        public string NomeProcesso { get; set; }

        [AtributoCampo(Nome = "RESTRICAOHORARIOSITUACAO")]
        public byte RestricaoHorarioSituacao { get; set; }

        [AtributoCampo(Nome = "SITUACAOPROCESSO")]
        public int SituacaoProcesso { get; set; }

        [AtributoCampo(Nome = "RESTRICAOHORARIOINICIO")]
        public TimeSpan? RestricaoHorarioInicio { get; set; }

        [AtributoCampo(Nome = "RESTRICAOHORARIOFIM")]
        public TimeSpan? RestricaoHorarioFim { get; set; }
    }
}
