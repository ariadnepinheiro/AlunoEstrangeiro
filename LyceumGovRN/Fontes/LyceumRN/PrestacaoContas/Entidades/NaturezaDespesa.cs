using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.NATUREZADESPESA", Nome = "PrestacaoContas.NATUREZADESPESA")]
    public class NaturezaDespesa : IEntity
    {
        [AtributoCampo(Nome = "NATUREZADESPESAID")]
        public int NaturezaDespesaId { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "CODIGOSEFAZ")]
        public string CodigoSefaz { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
