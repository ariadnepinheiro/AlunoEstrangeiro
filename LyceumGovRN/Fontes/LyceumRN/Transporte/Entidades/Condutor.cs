using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.CONDUTOR", Nome = "Transporte.CONDUTOR")]
    public class Condutor : IEntity
    {
        [AtributoCampo(Nome = "CONDUTORID")]
        public int CondutorId { get; set; }

        [AtributoCampo(Nome = "CPF")]
        public string Cpf { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }

        [AtributoCampo(Nome = "NUMEROCNH")]
        public string NumeroCnh { get; set; }

        [AtributoCampo(Nome = "DATAVALIDADECNH")]
        public DateTime DataValidadeCnh { get; set; }

        [AtributoCampo(Nome = "CATEGORIA")]
        public string Categoria { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}