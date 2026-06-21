using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class AreasConhecimento:IEntity
    {
        public int AreaConhecimentoId { get; set; }
        public string Descricao { get; set; }
    }
}
