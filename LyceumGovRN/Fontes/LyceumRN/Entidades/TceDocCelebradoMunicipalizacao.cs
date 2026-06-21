using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceDocCelebradoMunicipalizacao : IEntity
    {
        public int IdDocCelebradoMunicipalizacao { get; set; }

        public int IdMunicipalizacao { get; set; }

        public string Tipo { get; set; }

        public string Numero { get; set; }

        public DateTime DtCelebracao { get; set; }

        public DateTime DtValidade { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public string Observacao { get; set; }
    }
}
