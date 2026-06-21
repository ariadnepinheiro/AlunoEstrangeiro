using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.TipoCertificacao", Nome = "CertificacaoEscolar.TipoCertificacao")]
    public class TipoCertificacao : IEntity
    {
        [AtributoCampo(Nome = "TIPOCERTIFICACAOID")]
        public int TipoCertificacaoId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "PERMITEPOLO")]
        public bool PermitePolo { get; set; }

        [AtributoCampo(Nome = "PERMITECEJA")]
        public bool PermiteCeja { get; set; }

        [AtributoCampo(Nome = "PERMITETRANSPARENCIA")]
        public bool PermiteTransparencia { get; set; }

        [AtributoCampo(Nome = "ETAPAENSINO")]
        public string EtapaEnsino { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
