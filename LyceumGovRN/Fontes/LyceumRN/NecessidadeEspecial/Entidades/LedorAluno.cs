using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.NecessidadeEspecial.Entidades
{
    [AtributoTabela("NecessidadeEspecial.LedorAluno", Nome = "NecessidadeEspecial.LedorAluno")]
    public class LedorAluno : IEntity
    {
        [AtributoCampo(Nome = "LEDORALUNOID")]
        public int LedorAlunoId { get; set; }

        [AtributoCampo(Nome = "RECURSONECESSIDADEESPECIALID")]
        public int RecursoNecessidadeEspecialId { get; set; }

        [AtributoCampo(Nome = "ALUNOID")]
        public string AlunoId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "SEMESTRE")]
        public int Semestre { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

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
