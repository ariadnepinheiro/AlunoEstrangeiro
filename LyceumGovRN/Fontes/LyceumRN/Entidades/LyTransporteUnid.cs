using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyTransporteUnid : IEntity
    {
        public string TransporteTipo { get; set; }

        public string UnidadeFis { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
