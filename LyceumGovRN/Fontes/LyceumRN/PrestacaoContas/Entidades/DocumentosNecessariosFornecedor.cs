using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR", Nome = "PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR")]
    public class DocumentosNecessariosFornecedor : IEntity
    {
        [AtributoCampo(Nome = "MOTIVORETORNOID")]
        public int DocumentosNecessariosFornecedorId { get; set; }

        public string Descricao { get; set; }

        public int Periodicidade { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "TIPO")]
        public string Tipo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
