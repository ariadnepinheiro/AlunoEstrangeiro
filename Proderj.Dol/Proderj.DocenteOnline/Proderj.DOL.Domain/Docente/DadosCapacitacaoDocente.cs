using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class DadosCapacitacaoDocente
    {
        public virtual int Row { get; set; }
        public virtual string OferecidoSEEDUC { get; set; }
        public virtual string Capacitacao { get; set; }
        public virtual string TipoCurso { get; set; }
        public virtual string AreaConhecimento { get; set; }
        public virtual string NomeInstituicao { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual DateTime DataConclusao { get; set; }
        public virtual string Matricula { get; set; }
    }
}
