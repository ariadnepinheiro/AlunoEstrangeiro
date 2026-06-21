using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class FrequenciaAlunoTurma: IEntity
    {
        public string Disciplina { get; set; }
        public string Freq { get; set; }
        public int Faltas { get; set; }
        public int FaltasCompensadas { get; set; }
        public bool AtualizaFalta { get; set; }
    }
}
