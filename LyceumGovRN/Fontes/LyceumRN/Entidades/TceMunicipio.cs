using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceMunicipio : IEntity
    {
        public string Codigo { get; set; }

        [AtributoCampo(Nome = "UF_SIGLA")]
        public string UF { get; set; }

        public string Nome { get; set; }

        public string CodigoIbge { get; set; }
    }
}
