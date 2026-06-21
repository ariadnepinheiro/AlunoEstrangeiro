namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.MapeamentoAtributos;

    public class LyVinculo : IEntity
    {
        public Decimal Pessoa { get; set; }

        public Decimal Ordem { get; set; }

        public string Matricula { get; set; }

        public DateTime DataNomeacao { get; set; }

        public DateTime? DataDesativacao { get; set; }

        public DateTime StampAtualizacao { get; set; }

        public string Categoria { get; set; }

        public Decimal? ChCategoria { get; set; }

        public int? Vinculo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
