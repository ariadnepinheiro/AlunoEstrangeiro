namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceAvaliacaoCurriculoMinimoJustificativa : IEntity
    {
        public int IdAvaliacaoCurriculoMinimoJustificativa { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public int Subperiodo { get; set; }
       
        public string Justificativa { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}