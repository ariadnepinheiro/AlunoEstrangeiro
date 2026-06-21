using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.RESPOSTA", Nome = "AvaliacaoExterna.RESPOSTA")]
    public class Resposta : IEntity
    {
        [AtributoCampo(Nome = "RESPOSTAID")]
        public int RespostaId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "QUESTAOID")]
        public int QuestaoId { get; set; }

        [AtributoCampo(Nome = "RESPOSTA")]
        public int? resposta { get; set; }
        
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}