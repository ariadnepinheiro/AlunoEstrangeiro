namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceCompartilhada : IEntity
    {
        public string Censo { get; set; }

        public string CensoCompartilhada { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public DateTime DtCadastro { get; set; }

        public int IdCompartilhada { get; set; }

        public int? CedidasManha { get; set; }

        public int? CedidasTarde { get; set; }

        public int? CedidasNoite { get; set; }
       
        public string Matricula { get; set; }

        public string Nome { get; set; }

        public string RedeEnsino { get; set; }

        public string UnidadedeEnsino { get; set; }
    }
}