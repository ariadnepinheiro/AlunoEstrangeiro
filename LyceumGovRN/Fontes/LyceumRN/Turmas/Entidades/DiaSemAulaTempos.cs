using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.DIASEMAULA_TEMPOS", Nome = "Turma.DIASEMAULA_TEMPOS")]
    public class DiaSemAulaTempos : IEntity
    {
        [AtributoCampo(Nome = "DIASEMAULA_TEMPOSID")]
        public int DiaSemAulaTemposId { get; set; }

        [AtributoCampo(Nome = "DIASEMAULAID")]
        public int DiaSemAulaId { get; set; }

        public decimal Ano { get; set; }

        public decimal Semestre { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }

        public decimal Aula { get; set; }

        public string Faculdade { get; set; }

        [AtributoCampo(Nome = "DIA_SEMANA")]
        public decimal DiaSemana { get; set; }

        public string Turno { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
