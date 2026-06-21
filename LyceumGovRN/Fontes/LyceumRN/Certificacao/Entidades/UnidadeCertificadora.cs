using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.UNIDADECERTIFICADORA", Nome = "CertificacaoEscolar.UNIDADECERTIFICADORA")]
    public class UnidadeCertificadora : IEntity
    {
        [AtributoCampo(Nome = "UNIDADECERTIFICADORAID")]
        public int UnidadeCertificadoraId { get; set; }

        [AtributoCampo(Nome = "GRUPOUNIDADECERTIFICADORAID")]
        public int GrupoUnidadeCertificadoraId { get; set; }

        [AtributoCampo(Nome = "TIPO")]
        public string Tipo { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "DESCRICAOSITE")]
        public string DescricaoSite { get; set; }

        [AtributoCampo(Nome = "ENDERECO")]
        public string Endereco { get; set; }

        [AtributoCampo(Nome = "NUMERO")]
        public string Numero { get; set; }

        [AtributoCampo(Nome = "COMPLEMENTO")]
        public string Complemento { get; set; }

        [AtributoCampo(Nome = "BAIRRO")]
        public string Bairro { get; set; }

        [AtributoCampo(Nome = "CEP")]
        public string Cep { get; set; }

        [AtributoCampo(Nome = "MUNICIPIO")]
        public string Municipio { get; set; }

        [AtributoCampo(Nome = "TELEFONE")]
        public string Telefone { get; set; }

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
