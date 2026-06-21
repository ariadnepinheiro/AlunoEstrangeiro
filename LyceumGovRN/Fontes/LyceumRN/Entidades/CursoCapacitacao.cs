using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class CursoCapacitacao : IEntity
    {
        public int CursoCapacitacaoId { get; set; }
        public int AreaConhecimentoId { get; set; }
        public string NomeCurso { get; set; }
        public string NomeInstituicao { get; set; }
        public int CargaHoraria { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool OferecidoSeeduc { get; set; }
    }
}
