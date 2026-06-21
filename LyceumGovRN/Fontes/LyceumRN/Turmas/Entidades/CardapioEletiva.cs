using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.CARDAPIOELETIVA", Nome = "Turma.CARDAPIOELETIVA")]
    public class CardapioEletiva : IEntity
    {
        [AtributoCampo(Nome = "ALUNOPARTICIPANTEID")]
        public int CardapioEletivaId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public int Periodo { get; set; }

        [AtributoCampo(Nome = "CURSO")]
        public string Curso { get; set; }

        [AtributoCampo(Nome = "SERIE")]
        public int Serie { get; set; }

        [AtributoCampo(Nome = "VALIDADO")]
        public bool Validado { get; set; }

        [AtributoCampo(Nome = "USUARIOVALIDACAO")]
        public string UsuarioValidacao { get; set; }

        [AtributoCampo(Nome = "DATAVALIDACAO")]
        public DateTime? DataValidacao { get; set; }

        [AtributoCampo(Nome = "FINALIZADO")]
        public bool Finalizado { get; set; }

        [AtributoCampo(Nome = "USUARIOFINALIZACAO")]
        public string UsuarioFinalizacao { get; set; }

        [AtributoCampo(Nome = "DATAFINALIZACAO")]
        public DateTime? DataFinalizacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
