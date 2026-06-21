using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("TCE_COMPETENCIA_HABILIDADE_DOCENTE", Nome = "TCE_COMPETENCIA_HABILIDADE_DOCENTE")]
    public class TceCompetenciaHabilidadeDocente : IEntity
    {
        [AtributoCampo(Nome = "ID_COMPETENCIA_HABILIDADE_DOCENTE")]
        public int IdCompetenciaHabilidadeDocente { get; set; }

        [AtributoCampo(Nome = "ID_COMPETENCIA_HABILIDADE_ITEM")]
        public int IdCompetenciaHabilidadeItem { get; set; }

        public string Matricula { get; set; }

        public string Disciplina { get; set; }

        public int Subperiodo { get; set; }

        public string Turma { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        [AtributoCampo(Nome = "DATAFREQUENCIA")]
        public DateTime? DataFrequencia { get; set; } 

        [AtributoCampo(Nome = "NUM_FUNC")]
        public int? NumFunc { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DT_CADASTRO")]
        public DateTime DtCadastro { get; set; }
    }
}