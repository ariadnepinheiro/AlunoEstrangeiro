using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyUnidadeEnsinoCursos : IEntity
    {
        public string UnidadeEns { get; set; }

        public decimal Ordem { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Curriculo { get; set; }

        public string Ato { get; set; }

        public DateTime? DtImplantacao { get; set; }

        public DateTime? DtDo { get; set; }

        public string Observacoes { get; set; }

        public string Processo { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
