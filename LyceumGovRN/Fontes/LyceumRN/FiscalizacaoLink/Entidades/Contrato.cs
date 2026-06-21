using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    [AtributoTabela("FiscalizacaoLink.CONTRATO", Nome = "FiscalizacaoLink.CONTRATO")]
    public class Contrato : IEntity
    {
        [AtributoCampo(Nome = "CONTRATOID")]
        public int ContratoId { get; set; }

        [AtributoCampo(Nome = "TIPOLINKID")]
        public int TipoLinkId { get; set; }

        public string Numero { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "DATAPUBLICACAODO")]
        public DateTime? DataPublicacaoDo { get; set; } 

        [AtributoCampo(Nome = "DATATERMINO")]
        public DateTime? DataTermino { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
