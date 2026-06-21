using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.QUESTAO", Nome = "AvaliacaoExterna.QUESTAO")]
    public class Questao : IEntity
    {
        [AtributoCampo(Nome = "QUESTAOID")]
        public int QuestaoId { get; set; }

        [AtributoCampo(Nome = "PROVAID")]
        public int ProvaId { get; set; }

        [AtributoCampo(Nome = "HABILIDADEID")]
        public int HabilidadeId { get; set; }

        [AtributoCampo(Nome = "NUMERO")]
        public int Numero { get; set; }

        [AtributoCampo(Nome = "INDICEDIFICULDADE")]
        public decimal IndiceDificuldade { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEALTERNATIVAS")]
        public int QuantidadeAlternativas { get; set; }

        [AtributoCampo(Nome = "ALTERNATIVACORRETA")]
        public int AlternativaCorreta { get; set; }
        
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}