using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;
using System.Collections.Generic;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.TRANSCRICAOTURMA", Nome = "AvaliacaoExterna.TRANSCRICAOTURMA")]
    public class TranscricaoTurma : IEntity
    {
        [AtributoCampo(Nome = "TRANSCRICAOTURMAID")]
        public int TranscricaoTurmaId { get; set; }

        [AtributoCampo(Nome = "ETAPAID")]
        public int EtapaId { get; set; }

        [AtributoCampo(Nome = "ANO")] 
        public int Ano { get; set; }

        [AtributoCampo(Nome = "SEMESTRE")] 
        public int Semestre { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

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