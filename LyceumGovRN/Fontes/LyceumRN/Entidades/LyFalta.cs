namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class LyFalta : IEntity
    {
        public string Aluno { get; set; }
        public decimal? Ano { get; set; }
        public string Disciplina { get; set; }
        public decimal? Faltas { get; set; }
        public decimal? FaltasCompensadas { get; set; }
        public string Freq { get; set; }
        public decimal? Periodo { get; set; }
        public string Turma { get; set; }
    }
}