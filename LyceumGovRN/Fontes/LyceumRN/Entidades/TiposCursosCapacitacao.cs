using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TiposCursosCapacitacao:IEntity
    {
        public int TipoCursoCapacitacaoId { get; set; }
        public string Descricao { get; set; }
    }
}
