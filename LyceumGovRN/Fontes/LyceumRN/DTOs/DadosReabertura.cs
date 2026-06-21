using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosReabertura
    {
        public string Aluno { get; set; }

        public DateTime DataNascimento { get; set; }

        public DateTime DataEncerramento { get; set; }

        public DateTime DataReabertura { get; set; }

        public int AnoEncerramento { get; set; }

        public int AnoReabertura { get; set; }

        public int PeriodoEncerramento { get; set; }

        public int PeriodoReabertura { get; set; }

        public string CursoReabertura { get; set; }

        public string TurnoReabertura { get; set; }

        public int SerieReabertura { get; set; }

        public string CurriculoReabertura { get; set; }

        public string CursoAtual { get; set; }

        public string TurnoAtual { get; set; }

        public int SerieAtual { get; set; }

        public string CurriculoAtual { get; set; }

        public bool EnsinoReligioso { get; set; }

        public bool LinguaEstrangeira { get; set; }

        public string MotivoEncerramento { get; set; }

        public string MotivoReabertura { get; set; }

        public string UnidadeEnsino { get; set; }

        public string UsuarioResponsavel { get; set; }

        //Variaveis que serão alimentadas dentro do Validar
        public string TipoVaga { get; set; }
    }
}
