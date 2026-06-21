using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DadosFormacaoDocenteDTO
    {
        public string Escolaridade { get; set; }
        public string SituacaoCurso { get; set; }
        public string AreaCurso { get; set; }
        public string Curso { get; set; }
        public string FormacaoComplementacaoPedagogica { get; set; }
        public int AnoInicio { get; set; }
        public int AnoConclusao { get; set; }
        public string CodInstituicao { get; set; }
        public string Instituicao { get; set; }
        public string DocComprobatorio { get; set; }
        public string Matricula { get; set; }
    }
}
