using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class HistoricoImportacao : IEntity
    {
        public int HistoricoImportacaoId { get; set; }
        public string NomeArquivo { get; set; }
        public RN.HistoricoImportacao.StatusProcessamento StatusProcessamento { get; set; }
        public RN.HistoricoImportacao.TipoEntradaSistemaEnum TipoEntradaSistemaId { get; set; }
        public RN.HistoricoImportacao.TipoImportacaoEnum TipoImportacaoId { get; set; }
        public byte[] Arquivo { get; set; }
        public DateTime DataImportacao { get; set; }
        public int TotalRegistroImportado { get; set; }
        public string Usuario { get; set; }
    }
}