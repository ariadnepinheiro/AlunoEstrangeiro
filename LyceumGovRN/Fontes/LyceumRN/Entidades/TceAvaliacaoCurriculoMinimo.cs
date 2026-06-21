namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceAvaliacaoCurriculoMinimo : IEntity
    {
        public int IdAvaliacaoCurriculoMinimo { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public int Subperiodo { get; set; }

        public int Ordem { get; set; }

        public string Avaliacao { get; set; }

        public bool Habilitado { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; } 
    }
}