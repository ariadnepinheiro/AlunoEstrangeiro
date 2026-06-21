using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.USUARIOUNIDADECERTIFICADORA", Nome = "CertificacaoEscolar.USUARIOUNIDADECERTIFICADORA")]
    public class UsuarioUnidadeCertificadora : IEntity
    {
        [AtributoCampo(Nome = "USUARIOUNIDADECERTIFICADORAID")]
        public int UsuarioUnidadeCertificadoraId { get; set; }

        [AtributoCampo(Nome = "UNIDADECERTIFICADORAID")]
        public int UnidadeCertificadoraId { get; set; }       

        [AtributoCampo(Nome = "TIPO")]
        public string Usuario { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
