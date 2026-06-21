using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceDocumentoAluno : IEntity
    {
        public int IdDocumentoAluno { get; set; }

        public int IdDocumento { get; set; }

        public string Aluno { get; set; }

        public DateTime DtEntrega { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}
