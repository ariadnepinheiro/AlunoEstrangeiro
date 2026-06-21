using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.PROVA", Nome = "AvaliacaoExterna.PROVA")]
    public class Prova : IEntity
    {
        [AtributoCampo(Nome = "PROVAID")]
        public int ProvaId { get; set; }

        [AtributoCampo(Nome = "AVALIACAOID")]
        public int AvaliacaoId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEQUESTOES")]
        public int QuantidadeQuestoes { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}