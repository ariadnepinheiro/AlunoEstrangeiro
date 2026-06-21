using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.ORCAMENTOARQUIVO", Nome = "PrestacaoContas.ORCAMENTOARQUIVO")]
    public class OrcamentoArquivo : IEntity
    {
        [AtributoCampo(Nome = "ORCAMENTOARQUIVOID")]
        public int OrcamentoArquivoId { get; set; }

        [AtributoCampo(Nome = "EVENTOID")]
        public int EventoId { get; set; }
        
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
