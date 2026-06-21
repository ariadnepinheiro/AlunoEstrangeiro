using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvAnalise : IEntity
    {
        public int IdAnalise { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public bool Turno { get; set; }

        public bool Vaga { get; set; }

        public string AnaliseSuped { get; set; }

        public string MatriculaSuped { get; set; }

        public DateTime? DtAnaliseSuped { get; set; }

        public string AnaliseSuplan { get; set; }

        public string MatriculaSuplan { get; set; }

        public DateTime? DtAnaliseSuplan { get; set; }

        [AtributoCampo(Nome = "ANALISEDIESP")]
        public string AnaliseDiesp { get; set; }

        [AtributoCampo(Nome = "MATRICULADIESP")]
        public string MatriculaDiesp { get; set; }

        [AtributoCampo(Nome = "DATAANALISEDIESP")]
        public DateTime? DataAnaliseDiesp { get; set; }
    }
}
