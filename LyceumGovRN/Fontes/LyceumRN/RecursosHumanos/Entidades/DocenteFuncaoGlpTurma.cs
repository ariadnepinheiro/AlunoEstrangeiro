using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.DOCENTEFUNCAOGLP_TURMA", Nome = "RecursosHumanos.DOCENTEFUNCAOGLP_TURMA")]
    public class DocenteFuncaoGlpTurma
    {
        [AtributoCampo(Nome = "DOCENTEFUNCAOGLP_TURMAID")]
        public int DocenteFuncaoGlpTurmaId { get; set; }

        [AtributoCampo(Nome = "ID_DOCENTE_FUNCAO_GLP")]
        public decimal IdDocenteFuncaoGlp { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "DISCIPLINA")]
        public string Disciplina { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public decimal Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public decimal Periodo { get; set; }

        [AtributoCampo(Nome = "NUMFUNCCARENCIA")]
        public decimal NumFuncCarencia { get; set; }

        [AtributoCampo(Nome = "CARGAHORARIA")]
        public int CargaHoraria { get; set; }
    }
}
