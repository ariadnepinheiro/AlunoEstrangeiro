using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.TIPOCERTIFICACAO__UNIDADECERTIFICADORA", Nome = "CertificacaoEscolar.TIPOCERTIFICACAO__UNIDADECERTIFICADORA")]
    public class TipoCertificacaoUnidadeCertificadora : IEntity
    {
        [AtributoCampo(Nome = "TIPOCERTIFICACAOID")]
        public int TipoCertificacaoId { get; set; }

        [AtributoCampo(Nome = "UNIDADECERTIFICADORAID")]
        public int UnidadeCertificadoraId { get; set; }
        
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
