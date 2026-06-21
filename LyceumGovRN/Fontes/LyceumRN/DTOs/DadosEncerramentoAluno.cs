using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosEncerramentoAluno
    {
        public string Aluno { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Motivo { get; set; }

        public string MotivoDescricao { get; set; }

        public DateTime DtEncerramento { get; set; }

        public string CursoAtual { get; set; }

        public string NomeCursoAtual { get; set; }

        public string TurnoAtual { get; set; }

        public string NomeTurnoAtual { get; set; }

        public decimal SerieAtual { get; set; }

        public string NomeSerieAtual { get; set; }

        public string CurriculoAtual { get; set; }

        public string UnidadeEnsinoAtual { get; set; }

        public string NomeUnidadeEnsinoAtual { get; set; }

        public string Situacao { get; set; }
    }
}
