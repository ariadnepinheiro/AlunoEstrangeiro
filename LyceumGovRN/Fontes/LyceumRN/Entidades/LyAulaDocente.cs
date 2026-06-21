using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("LY_AULA_DOCENTE", Nome = "LY_AULA_DOCENTE")]
    public class LyAulaDocente : IEntity
    {
        [AtributoCampo(Nome = "NUM_FUNC")]
        public int NumFunc { get; set; }

        public string Turno { get; set; }

        public string Faculdade { get; set; }

        [AtributoCampo(Nome = "DIA_SEMANA")]
        public int DiaSemana { get; set; }

        public int Aula { get; set; }

        public string Disciplina { get; set; }

        public string Turma { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Tipo { get; set; }

        [AtributoCampo(Nome = "DATA_INICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATA_FIM")]
        public DateTime DataFim { get; set; }

        [AtributoCampo(Nome = "TIPO_DOCENTE")]
        public string TipoDocente { get; set; }

        [AtributoCampo(Nome = "STAMP_ATUALIZACAO")]
        public DateTime StampAtualizacao { get; set; }
    }
}
