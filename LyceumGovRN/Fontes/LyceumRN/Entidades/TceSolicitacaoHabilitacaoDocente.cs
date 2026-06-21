namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceSolicitacaoHabilitacaoDocente : IEntity
    {
        public string Agrupamento { get; set; }

        public DateTime DataAnalise { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool HabilitacaoGLP { get; set; }

        public bool HabilitacaoMatricula { get; set; }

        public int IdSolicitacaoHabilitacaoDocente { get; protected set; }

        public string Motivo { get; set; }

        public decimal NumFunc { get; set; }

        public decimal? NumFuncSubstituido { get; set; }

        public string SegmentoAtuacao { get; set; }

        public string Status { get; set; }

        public string TipoSubstituicao { get; set; }

        public string UnidadeEns { get; set; }
    }
}