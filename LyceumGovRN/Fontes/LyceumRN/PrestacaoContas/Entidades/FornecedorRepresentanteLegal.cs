using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.FORNECEDORREPRESENTANTELEGAL", Nome = "PrestacaoContas.FORNECEDORREPRESENTANTELEGAL")]
    public class FornecedorRepresentanteLegal : IEntity
    {
        [AtributoCampo(Nome = "FORNECEDORREPRESENTANTELEGALID")]
        public int? FornecedorRepresentanteLegalId { get; set; }

        [AtributoCampo(Nome = "FORNECEDORID")]
        public int FornecedorId { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }

        [AtributoCampo(Nome = "CPF")]
        public string Cpf { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}