using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class DadosTransferencia
    {
        public string Aluno { get; set; }

        public string Ano { get; set; }

        public string SituacaoAluno { get; set; }

        public string UnidadeEnsino { get; set; }

        public string UnidadeFisica { get; set; }

        public string MotivoTransferencia { get; set; }

        public string EnsinoReligioso { get; set; }

        public string LinguaEstrangeira { get; set; }

        public string UsuarioResponsavel { get; set; }

        public string NecessidadeEspecial { get; set; }

        public DateTime DataNascimento { get; set; }

        public bool PossuiTurmaConcomitante { get; set; }

        public string TurnoAtualTurmaConcomitante { get; set; }

        //Dados da matricula origem:       

        public string SemestreAtual { get; set; }

        public string TurmaAtual { get; set; }

        public string NumChamadaAtual { get; set; }

        public string GradeIdAtual { get; set; }

        public string TurnoAtual { get; set; }

        public string CursoAtual { get; set; }

        public string SerieAtual { get; set; }       

        //Dados da matricula destino:
        public string SemestreDestino { get; set; }

        public string TurnoDestino { get; set; }

        public string CursoDestino { get; set; }

        public string TipoCursoDestino { get; set; }

        public string CurriculoDestino { get; set; }

        public string SerieDestino { get; set; }

        public string TurmaDestino { get; set; }

        public string GradeIdDestino { get; set; }

        public string TipoEnsProfissionalizanteDestino { get; set; }

        public List<string> ListaDisciplinasTurmaDestino { get; set; }
    }
}
