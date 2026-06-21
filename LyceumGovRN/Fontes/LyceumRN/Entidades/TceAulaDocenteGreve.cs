namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceAulaDocenteGreve : IEntity
    {
        public int Codigo { get; set; }

        public int NumFunc { get; set; }

        public string Turno { get; set; }

        public string Faculdade { get; set; }

        public int DiaSemana { get; set; }

        public int Aula { get; set; }

        public string Disciplina { get; set; }

        public string Turma { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public DateTime DataInicio { get; set; }

        public string Tipo { get; set; }

        public string MatriculaSubstituto { get; set; }

        public DateTime DataInicioSubstituicao { get; set; }

        public DateTime DataFimSubstituicao { get; set; }

        public string Matricula { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool ativo { get; set; }

        public string ValorGLP { get; set; }
    }
}