using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Certificacao.DTOs
{
    public class PainelCertificacao 
    {
        public int EnccejaRequerimentoId { get; set; }

        public int SituacaoEnccejaRequerimentoId { get; set; }

        public int? MotivoIndeferidoId { get; set; }

        public DateTime DataVerificacao { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public DateTime DataEntrega { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public bool Entregue { get; set; }
    }
}
