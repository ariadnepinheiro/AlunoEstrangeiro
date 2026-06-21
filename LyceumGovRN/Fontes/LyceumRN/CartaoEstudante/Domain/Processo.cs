using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Enum;

namespace Techne.Lyceum.RN.CartaoEstudante.Domain
{         
    public class Processo
    {
        public int Id { get; set; }                     
        
        public string Nome { get; set; }

        public SituacaoProcessoEnum SituacaoProcesso { get; set; }

        public int RestricaoHorarioSituacao { get; set; }
        
        public TimeSpan? RestricaoHorarioInicio { get; set; }

        public TimeSpan? RestricaoHorarioFim { get; set; }

        public bool TemRestricaoHorario
        {
            get
            {
                return Convert.ToBoolean(this.RestricaoHorarioSituacao);
            }

            set
            {
                this.RestricaoHorarioSituacao = value ? 1 : 0;
            }
        }

        public Processo(){}
    }
}
