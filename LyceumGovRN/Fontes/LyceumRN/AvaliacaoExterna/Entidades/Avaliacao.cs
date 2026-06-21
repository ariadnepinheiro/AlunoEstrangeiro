using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.AVALIACAO", Nome = "AvaliacaoExterna.AVALIACAO")]
    public class Avaliacao : IEntity
    {
        [AtributoCampo(Nome = "AVALIACAOID")]
        public int AvaliacaoId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "TIPOAVALIACAOID")]
        public int TipoAvaliacaoID { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano{ get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool? Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro{ get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
