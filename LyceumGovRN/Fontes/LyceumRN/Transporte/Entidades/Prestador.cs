using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.PRESTADOR", Nome = "Transporte.PRESTADOR")]
    public class Prestador : IEntity
    {
        [AtributoCampo(Nome = "PRESTADORID")]
        public int PrestadorId { get; set; }

        [AtributoCampo(Nome = "CNPJ")]
        public string Cnpj { get; set; }

        [AtributoCampo(Nome = "CPF")]
        public string Cpf { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }

        [AtributoCampo(Nome = "TELEFONE")]
        public string Telefone { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
