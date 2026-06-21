using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.COMPONENTE", Nome = "AvaliacaoExterna.COMPONENTE")]
    public class Componente : IEntity
    {
        [AtributoCampo(Nome = "COMPONENTEID")]
        public int ComponenteId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }
        
        [AtributoCampo(Nome = "ATIVO")]
        public bool? Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
