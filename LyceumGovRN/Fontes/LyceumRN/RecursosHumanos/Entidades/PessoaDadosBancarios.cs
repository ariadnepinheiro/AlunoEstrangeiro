using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.PESSOADADOSBANCARIOS", Nome = "RecursosHumanos.PESSOADADOSBANCARIOS")]
    public class PessoaDadosBancarios : IEntity
    {
        [AtributoCampo(Nome = "PESSOADADOSBANCARIOSID")]
        public int PessoaDadosBancariosId { get; set; }

        [AtributoCampo(Nome = "PESSOAID")]
        public decimal PessoaId { get; set; }

        [AtributoCampo(Nome = "BANCO")]
        public decimal Banco { get; set; }

        [AtributoCampo(Nome = "AGENCIA")]
        public string Agencia { get; set; }

        [AtributoCampo(Nome = "CONTABANCO")]
        public string ContaBanco { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
