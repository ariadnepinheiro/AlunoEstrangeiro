using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.CARDAPIOELETIVADISCIPLINA", Nome = "Turma.CARDAPIOELETIVADISCIPLINA")]
    public class CardapioEletivaDisciplina : IEntity
    {
        [AtributoCampo(Nome = "CARDAPIOELETIVADISCIPLINAID")]
        public int CardapioEletivaDisciplinaId { get; set; }

        [AtributoCampo(Nome = "ALUNOPARTICIPANTEID")]
        public int CardapioEletivaId { get; set; }

        [AtributoCampo(Nome = "TURNO")]
        public string Turno { get; set; }

        [AtributoCampo(Nome = "DISCIPLINA")]
        public string Disciplina { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

