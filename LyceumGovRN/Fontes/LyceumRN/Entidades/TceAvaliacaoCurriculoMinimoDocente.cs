namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceAvaliacaoCurriculoMinimoDocente : IEntity
    {
        public int IdAvaliacaoCurriculoMinimoDocente { get; set; }

        public int IdAvaliacaoCurriculoMinimo { get; set; }

        public string Resposta { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}