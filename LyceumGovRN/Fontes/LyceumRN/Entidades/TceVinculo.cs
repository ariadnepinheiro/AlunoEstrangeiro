using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceVinculo : IEntity
    {
        public int IdVinculo { get; set; }

        public Date DtInicio { get; set; }

        public Date DtFim { get; set; }

        public bool Principal { get; set; }

        public string UnidadeEnsino { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
