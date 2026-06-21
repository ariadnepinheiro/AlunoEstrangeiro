namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class TceCompetenciaHabilidadeGrupo : IEntity
    {
        public int IdCompetenciaHabilidadeGrupo { get; set; }

        public int Ano { get; set; }

        public string Curso { get; set; }

        public string Disciplina { get; set; }

        public string DtAlteracao { get; set; }

        public string DtCadastro { get; set; }

        public string Grupo { get; set; }

        public string Matricula { get; set; }

        public string Modalidade { get; set; }

        public int Ordem { get; set; }

        public int Periodo { get; set; }

        public int Serie { get; set; }

        public int Subperiodo { get; set; }

        public string TipoCurso { get; set; }

        public string Tipo { get; set; }
    }
}