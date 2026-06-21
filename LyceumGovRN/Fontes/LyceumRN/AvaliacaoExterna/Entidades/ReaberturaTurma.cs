using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.AvaliacaoExterna.Entidades
{
    [AtributoTabela("AvaliacaoExterna.REABERTURATURMA", Nome = "AvaliacaoExterna.REABERTURATURMA")]
    public class ReaberturaTurma : IEntity
    {
        [AtributoCampo(Nome = "REABERTURATURMAID")]
        public int ReaberturaTurmaId { get; set; }

        [AtributoCampo(Nome = "ETAPAID")]
        public int EtapaId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; } 

        [AtributoCampo(Nome = "SEMESTRE")]
        public int Semestre { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "SOLICITANTEID")]
        public string SolicitanteId { get; set; }
       
        [AtributoCampo(Nome = "DATASOLICITACAO")]
        public DateTime DataSolicitacao { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "STATUSID")]
        public int StatusId { get; set; }

        [AtributoCampo(Nome = "APROVADORID")]
        public string AprovadorId { get; set; }

        [AtributoCampo(Nome = "DATALIBERACAO")]
        public DateTime DataLiberacao { get; set; }

        [AtributoCampo(Nome = "DATAFECHAMENTO")]
        public DateTime DataFechamento { get; set; }
         
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro{ get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }  
    }
}
