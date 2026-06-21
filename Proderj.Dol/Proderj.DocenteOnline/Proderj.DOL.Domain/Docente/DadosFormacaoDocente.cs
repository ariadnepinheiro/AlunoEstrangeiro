using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public enum TipoFormacaoEnum 
    {
        Graduacao = 1,
        PosGraduacao = 2
    }

    public class DadosFormacaoDocente
    {
        public virtual int Row { get; set; }
        public virtual string Escolaridade { get; set; }        
        public virtual string SituacaoCurso { get; set; }
        public virtual string AreaCurso { get; set; }
        public virtual string Curso { get; set; }
        public virtual string FormacaoComplementacaoPedagogica { get; set; }
        public virtual int AnoInicio { get; set; }
        public virtual int AnoConclusao { get; set; }
        public virtual string CodInstituicao { get; set; }
        public virtual string Instituicao { get; set; }
        public virtual string DocComprobatorio { get; set; }
        public virtual string Matricula { get; set; }
    }
}
