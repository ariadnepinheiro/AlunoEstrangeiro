using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.AULASDESALOCADAS", Nome = "RecursosHumanos.AULASDESALOCADAS")]
    public class AulasDesalocadas : IEntity
    {
        [AtributoCampo(Nome = "AULASDESALOCADASID")]
        public int AulasDesalocadasId { get; set; }

        [AtributoCampo(Nome = "DOCENTECANDIDATOID")]
        public int DocenteCandidatoId { get; set; }

        [AtributoCampo(Nome = "NUM_FUNC")]
        public decimal NumFunc { get; set; }

        [AtributoCampo(Nome = "TIPO_AULA")]
        public string TipoAula { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public decimal Ano { get; set; }

        [AtributoCampo(Nome = "SEMESTRE")]
        public decimal Semestre { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "DISCIPLINA")]
        public string Disciplina { get; set; }

        [AtributoCampo(Nome = "TURNO")]
        public string Turno { get; set; }

        [AtributoCampo(Nome = "FACULDADE")]
        public string Faculdade { get; set; }

        [AtributoCampo(Nome = "DIA_SEMANA")]
        public int DiaSemana { get; set; }

        [AtributoCampo(Nome = "AULA")]
        public int Aula { get; set; }

        [AtributoCampo(Nome = "DATA_INICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }
    }
}
