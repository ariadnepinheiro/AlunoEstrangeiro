using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("LY_DOCENTE_FUNCAO_GLP", Nome = "LY_DOCENTE_FUNCAO_GLP")]
    public class LyDocenteFuncaoGlp
    {
        [AtributoCampo(Nome = "ID_DOCENTE_FUNCAO_GLP")]
        public decimal IdDocenteFuncaoGlp { get; set; }

        public string Matricula { get; set; }

        [AtributoCampo(Nome = "FUNCAO_GLP")]
        public string FuncaoGlp { get; set; }

        public decimal? Ano { get; set; }

        public decimal? Mes { get; set; }

        public string Status { get; set; }

        [AtributoCampo(Nome = "UNIDADE_ENS")]
        public string UnidadeEns { get; set; }

        public DateTime? Data { get; set; }

        [AtributoCampo(Nome = "GLP_SOLICITADA")]
        public decimal? GlpSolicitada { get; set; }

        [AtributoCampo(Nome = "GLP_USADA")]
        public decimal? GlpUsada { get; set; }

        [AtributoCampo(Nome = "GLP_CANCELADA")]
        public decimal? GlpCancelada { get; set; }

        public string Agrupamento { get; set; }

        public decimal? Prazo { get; set; }

        [AtributoCampo(Nome = "CHLIVRE")]
        public decimal? ChLivre { get; set; }

        [AtributoCampo(Nome = "CHLIVREMUNICIPIO")]
        public decimal? ChLivreMunicipio { get; set; }

        [AtributoCampo(Nome = "CH_GLPID")]
        public decimal? ChGlpId { get; set; }

        [AtributoCampo(Nome = "RESERV01")]
        public string Reserv01 { get; set; }

        [AtributoCampo(Nome = "USUARIOSOLICITACAOID")]
        public string UsuarioSolicitacaoId { get; set; }

        [AtributoCampo(Nome = "DATASOLICITACAO")]
        public DateTime? DataSolicitacao { get; set; } 

        [AtributoCampo(Nome = "USUARIOANALISEID")]
        public string UsuarioAnaliseId { get; set; }

        [AtributoCampo(Nome = "DATAANALISE")]
        public DateTime? DataAnalise { get; set; }       
    }
}
