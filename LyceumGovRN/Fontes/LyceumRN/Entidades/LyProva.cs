namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class LyProva : IEntity
    {
        public decimal? Ano { get; set; }

        public string Disciplina { get; set; }

        public string Nome { get; set; }

        public string NotaMax { get; set; }

        public decimal? Ordem { get; set; }

        public string Prova { get; set; }

        public decimal? Semestre { get; set; }

        public decimal? Subperiodo { get; set; }

        public string Turma { get; set; }
    }
}