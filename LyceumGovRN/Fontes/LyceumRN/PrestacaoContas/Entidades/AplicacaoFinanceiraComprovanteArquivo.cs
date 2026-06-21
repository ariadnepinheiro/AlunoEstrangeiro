using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO", Nome = "PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO")]
    public class AplicacaoFinanceiraComprovanteArquivo : IEntity
    {
        [AtributoCampo(Nome = "APLICACAOFINANCEIRACOMPROVANTEARQUIVOID")]
        public int AplicacaoFinanceiraComprovanteArquivoId { get; set; }

        [AtributoCampo(Nome = "APLICACAOFINANCEIRAID")]
        public int AplicacaoFinanceiraId { get; set; }

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
