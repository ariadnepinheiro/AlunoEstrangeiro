using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    public class WsStatusFoto
    {
        public int WsStatusFotoId { get; set; }

        public int ExecucaoIntegradorId { get; set; }

        public int NumeroRegistro { get; set; }

        public string Matricula { get; set; }

        public string IdBeneficiario { get; set; }

        public string NumeroCartao { get; set; }

        public string NumeroChip { get; set; }

        public bool GerouRequisicao { get; set; }

        public int? CriticaFotoId { get; set; }

        public int? IdRequisicao { get; set; }

        public int? IdSolitacao { get; set; }

        public int? TipoRequisicao { get; set; }

        public DateTime? DataRequisicao { get; set; }

        public string DataRequisicaoWs { get; set; }

        public int? IdFoto { get; set; }

        public int? OrigemFotoId { get; set; }

        public DateTime? DataInclucao { get; set; }

        public string DataInclucaoWs { get; set; }

        public string StatusFoto { get; set; }

        public DateTime? DataStatus { get; set; }

        public string DataStatusWs { get; set; }

        public int? MotivoRejeicaoFotoId { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
