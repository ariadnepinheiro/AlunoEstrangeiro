using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades
{
    public class UnidadeEnsinoCursoTurnoInscricao
    {
        public int UnidadeEnsinoCursoTurnoInscricaoId { get; set; }
        public string UnidadeEnsinoId { get; set; }
        public int InscricaoId { get; set; }
        public string CursoId { get; set; }
        public string TurnoId { get; set; }
        public string IP { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
