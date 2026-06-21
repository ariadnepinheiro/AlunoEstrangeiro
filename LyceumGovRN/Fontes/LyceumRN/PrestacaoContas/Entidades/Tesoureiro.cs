using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.TESOUREIRO", Nome = "PrestacaoContas.TESOUREIRO")]
    public class Tesoureiro : IEntity
    {
        [AtributoCampo(Nome = "TESOUREIROID")]
        public int TesoureiroId { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }

        [AtributoCampo(Nome = "CPF")]
        public string Cpf { get; set; }

        [AtributoCampo(Nome = "RG")]
        public string Rg { get; set; }

        [AtributoCampo(Nome = "ENDERECO")]
        public string Endereco { get; set; }

        [AtributoCampo(Nome = "NUMERO")]
        public string Numero { get; set; }

        [AtributoCampo(Nome = "COMPLEMENTO")]
        public string Complemento { get; set; }

        [AtributoCampo(Nome = "BAIRRO")]
        public string Bairro { get; set; }

        [AtributoCampo(Nome = "MUNICIPIOID")]
        public string MunicipioId { get; set; }

        [AtributoCampo(Nome = "CEP")]
        public string Cep { get; set; }

        [AtributoCampo(Nome = "EMAIL")]
        public string Email { get; set; }

        [AtributoCampo(Nome = "TELEFONE")]
        public string Telefone { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
