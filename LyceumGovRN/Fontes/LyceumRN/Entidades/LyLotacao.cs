namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
    public class LyLotacao : IEntity
    {
        public Decimal Pessoa { get; set; }

        public string Matricula { get; set; }

        public Decimal Ordem { get; set; }

        public string Funcao { get; set; }

        public string TipoLotacao { get; set; }

        public string Turno { get; set; }

        public DateTime? DataDesativacao { get; set; }

        public string AtoOficial { get; set; }

        public string RespDocumentacao { get; set; }

        public string UnidadeFis { get; set; }

        public DateTime DataNomeacao { get; set; }

        public DateTime? DataNomeacaoDo { get; set; }

        public DateTime? DataDesativacaoDo { get; set; }

        public string TipoDesativacao { get; set; }

        public string UnidadeEns { get; set; }

        public string Nucleo { get; set; }

        public string Setor { get; set; }

        public string Categoria { get; set; }

        public string Readaptado { get; set; }

        public DateTime? DtInicioReadaptacao { get; set; }

        public DateTime? DtFimReadaptacao { get; set; }

        public string MotivoReadaptacao { get; set; }

        public string Usuario { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
