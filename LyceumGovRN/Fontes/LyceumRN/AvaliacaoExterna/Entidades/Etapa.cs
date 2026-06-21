using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;
namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.ETAPA", Nome = "AvaliacaoExterna.ETAPA")]
    public class Etapa : IEntity
    {
        [AtributoCampo(Nome = "ETAPAID")]
        public int EtapaId { get; set; }

        [AtributoCampo(Nome = "PROVAID")]
        public int ProvaId { get; set; } 

        [AtributoCampo(Nome = "CURSO")]
        public string Curso { get; set; }

        [AtributoCampo(Nome = "SERIE")]
        public int Serie { get; set; }

        [AtributoCampo(Nome = "INICIOREALIZACAO")]
        public DateTime InicioRealizacao { get; set; } 

        [AtributoCampo(Nome = "FIMREALIZACAO")]
        public DateTime FimRealizacao { get; set; } 

        [AtributoCampo(Nome = "INICIOTRANSCRICAO")]
        public DateTime InicioTranscricao { get; set; }
         
        [AtributoCampo(Nome = "FIMTRANSCRICAO")]
        public DateTime FimTranscricao { get; set; }       

        [AtributoCampo(Nome = "ATIVO")]
        public bool? Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
