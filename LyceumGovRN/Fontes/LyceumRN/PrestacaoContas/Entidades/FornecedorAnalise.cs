using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.FORNECEDORANALISE", Nome = "PrestacaoContas.FORNECEDORANALISE")]
    public class FornecedorAnalise : IEntity
    {
        [AtributoCampo(Nome = "FORNECEDORANALISEID")]
        public int FornecedorAnaliseId { get; set; }

        [AtributoCampo(Nome = "FORNECEDORID")]
        public int FornecedorId { get; set; }

        [AtributoCampo(Nome = "MOTIVOREPROVACAOFORNECEDORID")]
        public int? MotivoReprovacaoFornecedorId { get; set; }

        [AtributoCampo(Nome = "APROVADA")]
        public bool Aprovada { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
