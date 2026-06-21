using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosSalaAlternativa : IEntity
    {
        public string FACULDADE { get; set; }

        public string DEPENDENCIA { get; set; }

        public string DESCRICAO { get; set; }

        public string TIPO_DEPEND { get; set; }

        public int quatidade { get; set; }

        public string NUM_ALUNOS { get; set; }

        public string ATIVA { get; set; }

        public string TIPO_DEPEND_ANTERIOR { get; set; }

        public string ATIVA_ANTERIOR { get; set; }

        public string OBS { get; set; }

        public string EDIFICACAO { get; set; }

        public string PAVIMENTO { get; set; }

        public string AREA { get; set; }

        public string CAD_SALA_AULA { get; set; }

        public string SALA_ANEXA { get; set; } 

        public string MATRICULA { get; set; }

        public string DT_CADASTRO { get; set; }

        public string DT_ALTERACAO { get; set; }
    }

}
