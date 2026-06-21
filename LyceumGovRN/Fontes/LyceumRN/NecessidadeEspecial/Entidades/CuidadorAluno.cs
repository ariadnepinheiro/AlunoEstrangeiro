using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.NecessidadeEspecial.Entidades
{
    [AtributoTabela("NecessidadeEspecial.CuidadorAluno", Nome = "NecessidadeEspecial.CuidadorAluno")]
    public class CuidadorAluno : IEntity
    {
        [AtributoCampo(Nome = "CUIDADORALUNOID")]
        public int CuidadorAlunoId { get; set; }

        [AtributoCampo(Nome = "ALUNOID")]
        public string AlunoId { get; set; }

        [AtributoCampo(Nome = "RECURSONECESSIDADEESPECIALID")]
        public int RecursoNecessidadeEspecialId { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
