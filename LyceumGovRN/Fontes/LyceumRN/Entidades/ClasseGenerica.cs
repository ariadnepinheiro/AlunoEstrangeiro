using System;
using Seeduc.Infra.Entities;
using System.Collections.Generic;

namespace Techne.Lyceum.RN.Entidades
{
    public class ClasseGenerica:IEntity
    {
        public IList<int> Filhos { get; set; }
        public int Pai { get; set; }
    }
}
