using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosIrmao
    {
        public int? InscricaoAlunoId { get; set; }

        public int? NumeroInscricao { get; set; }

        public string MatriculaConexao { get; set; }

        public string NomeCompl { get; set; }

        public DateTime? DataNascimento { get; set; }

        //Dados para preenchimento da tela

        public string UsuarioResponsavel { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        //Dados da escola atual de alunos para transferencia

        public string CensoEscolaAtual { get; set; }

        public string EscolaAtual { get; set; }

        public int? SerieAtual { get; set; }

        public string CursoDescricaoAtual { get; set; }

        public string TurnoDescricaoAtual { get; set; }
    }
}
