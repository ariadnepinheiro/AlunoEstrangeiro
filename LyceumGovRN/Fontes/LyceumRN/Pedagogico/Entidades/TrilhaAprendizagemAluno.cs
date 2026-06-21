using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    [AtributoTabela("Pedagogico.TRILHAAPRENDIZAGEM_ALUNO", Nome = "Pedagogico.TRILHAAPRENDIZAGEM_ALUNO")]
    public class TrilhaAprendizagemAluno: IEntity
    {
        [AtributoCampo(Nome = "TRILHAAPRENDIZAGEM_ALUNOID")]
        public int TrilhaAprendizagemAlunoId { get; set; }

        [AtributoCampo(Nome = "TRILHAAPRENDIZAGEMID")]
        public int TrilhaAprendizagemId { get; set; }

        [AtributoCampo(Nome = "AnoOferta")]
        public int AnoOferta { get; set; }

        public string Aluno { get; set; }

        public string Censo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
