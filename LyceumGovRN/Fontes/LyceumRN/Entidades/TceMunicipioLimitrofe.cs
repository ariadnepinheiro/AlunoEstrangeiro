using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceMunicipioLimitrofe : IEntity
    {
        public int IdMunicipioLimitrofe { get; set; }

        public string CodigoMunicipioLimitrofe { get; set; }

        public string Uf { get; set; }

        public string CodigoMunicipio { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
