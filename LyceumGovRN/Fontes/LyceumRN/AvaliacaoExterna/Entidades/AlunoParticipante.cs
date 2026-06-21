using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.ALUNOPARTICIPANTE", Nome = "AvaliacaoExterna.ALUNOPARTICIPANTE")]
    public class AlunoParticipante : IEntity
    {
        [AtributoCampo(Nome = "ALUNOPARTICIPANTEID")]
        public int AlunoParticipanteId { get; set; }

        [AtributoCampo(Nome = "PROVAID")]
        public int ProvaId { get; set; }

        [AtributoCampo(Nome = "COMPONENTEID")]
        public int ComponenteId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "SITUACAOPARTICIPANTEID")]
        public int SituacaoParticipanteId { get; set; }

        [AtributoCampo(Nome = "DATAPARTICIPACAO")]
        public DateTime? DataParticipacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
