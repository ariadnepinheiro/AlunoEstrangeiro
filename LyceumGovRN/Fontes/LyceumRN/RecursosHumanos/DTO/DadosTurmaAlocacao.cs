using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.RecursosHumanos.DTO
{
    public class DadosTurmaAlocacao
    {
        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Turma { get; set; }

        public string Turno { get; set; }

        public string Disciplina { get; set; }

        public string DisciplinaMultipla { get; set; }

        public string Faculdade { get; set; }

        public string TipoAula { get; set; }

        public string TipoDocente { get; set; }

        public int DiaSemana { get; set; }

        public string DiaSemanaDescricao { get; set; }

        public DateTime HoraInicio { get; set; }

        public DateTime HoraFim { get; set; }

        public int Aula { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public decimal NumFuncAnterior { get; set; }

        //Campos para validação de Quadro de horario
        public Ly_hor_aula.Row DadosHoraAula { get; set; }

        public Ly_turma.Row DadosTurma { get; set; }       

        public string NomeDisciplina { get; set; }

    }
}
