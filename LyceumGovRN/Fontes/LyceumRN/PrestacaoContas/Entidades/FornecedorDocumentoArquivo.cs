using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO", Nome = "PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO")]
    public class FornecedorDocumentoArquivo : IEntity
    {
        [AtributoCampo(Nome = "FORNECEDORDOCUMENTOARQUIVOID")]
        public int FornecedorDocumentoArquivoId { get; set; }

        [AtributoCampo(Nome = "DOCUMENTOSFORNECEDORID")]
        public int DocumentosFornecedorId { get; set; }

        [AtributoCampo(Nome = "CHAVEARQUIVO")]
        public string ChaveArquivo { get; set; }

        [AtributoCampo(Nome = "ARQUIVO")]
        public byte[] Arquivo { get; set; }

        [AtributoCampo(Nome = "TIPOARQUIVO")]
        public string TipoArquivo { get; set; }

        [AtributoCampo(Nome = "NOMEARQUIVO")]
        public string NomeArquivo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
