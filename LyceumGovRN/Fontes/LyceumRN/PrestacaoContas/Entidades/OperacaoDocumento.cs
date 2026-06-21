using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.OPERACAODOCUMENTOS", Nome = "PrestacaoContas.OPERACAODOCUMENTOS")]
    public class OperacaoDocumentos : IEntity
    {

        [AtributoCampo(Nome = "OPERACAODOCUMENTOSID")]
        public int OperacaoDocumentosId { get; set; }

        [AtributoCampo(Nome = "OPERACAOID")]
        public int OperacaoId { get; set; }

        [AtributoCampo(Nome = "DOCUMENTOSNECESSARIOSOPERACOESID")]
        public int DocumentosNecessariosOperacoesId { get; set; }

        [AtributoCampo(Nome = "DATAENVIO")]
        public DateTime DataEnvio { get; set; }

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
