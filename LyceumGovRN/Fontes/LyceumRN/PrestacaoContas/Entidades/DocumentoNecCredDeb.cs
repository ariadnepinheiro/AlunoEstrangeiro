using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.DOCUMENTOSNECESSARIOSOPERACOES", Nome = "PrestacaoContas.DOCUMENTOSNECESSARIOSOPERACOES")]
    public class DocumentoNecCredDeb : IEntity
    {
        [AtributoCampo(Nome = "DOCUMENTOSNECESSARIOSOPERACOESID")]
        public int DocumentosNecessariosOperacoesID { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool? Ativo { get; set; }
        
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public string DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
