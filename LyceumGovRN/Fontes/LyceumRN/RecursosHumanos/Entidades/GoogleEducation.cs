using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.GOOGLEEDUCATION", Nome = "RecursosHumanos.GOOGLEEDUCATION")]
    public class GoogleEducation : IEntity
    {
        [AtributoCampo(Nome = "GOOGLEEDUCATIONID")]
        public int GoogleEducationId { get; set; }

        [AtributoCampo(Nome = "PESSOA")]
        public decimal Pessoa { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "EMAIL")]
        public string Email { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; } 

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
