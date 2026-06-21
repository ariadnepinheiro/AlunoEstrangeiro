using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.CH_GLP", Nome = "RecursosHumanos.CH_GLP")]
    public class ChGlp : IEntity
    {
        [AtributoCampo(Nome = "CH_GLPID")]
        public int ChGlpId { get; set; }

        [AtributoCampo(Nome = "NR_MATRICULAS")]
        public int NrMatriculas { get; set; }

        [AtributoCampo(Nome = "AGRUPAMENTOCARGOSID")]
        public int AgrupamentoCargosId { get; set; }

        [AtributoCampo(Nome = "CH_GRUPO")]
        public int ChGrupo { get; set; }

        [AtributoCampo(Nome = "FUNCAO")]
        public string Funcao { get; set; }

        [AtributoCampo(Nome = "AGRUPAMENTOCARGOSID_2")]
        public int? AgrupamentoCargosId2 { get; set; }

        [AtributoCampo(Nome = "CH_GRUPO_2")]
        public int? ChGrupo2 { get; set; }

        [AtributoCampo(Nome = "FUNCAO_2")]
        public string Funcao2 { get; set; }

        [AtributoCampo(Nome = "CH_SEMANAL_TOTAL")]
        public int ChSemanalTotal { get; set; }

        [AtributoCampo(Nome = "CH_GLP")]
        public int Ch_Glp { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
