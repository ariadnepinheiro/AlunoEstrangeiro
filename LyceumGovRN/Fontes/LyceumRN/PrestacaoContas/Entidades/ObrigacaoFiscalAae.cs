using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.OBRIGACAOFISCALAAE", Nome = "PrestacaoContas.OBRIGACAOFISCALAAE")]
    public class ObrigacaoFiscalAae : IEntity
    {
        [AtributoCampo(Nome = "OBRIGACAOFISCALAAEID")]
        public int ObrigacaoFiscalAaeId { get; set; }

        [AtributoCampo(Nome = "DECLARACAOAAEID")]
        public int DeclaracaoAaeId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "anobase")]
        public int AnoBase { get; set; }

        [AtributoCampo(Nome = "mes")]
        public int Mes { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}