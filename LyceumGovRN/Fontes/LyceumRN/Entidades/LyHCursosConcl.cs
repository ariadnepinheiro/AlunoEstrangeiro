using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyHCursosConcl : IEntity
    {
        public string Curso { get; set; }
        public string Turno { get; set; }
        public string Curriculo { get; set; }
        public string Aluno { get; set; }
        public DateTime DtEncerramento { get; set; }
        public string Motivo { get; set; }
        public string OutraFaculdade { get; set; }
        public Decimal AnoIngresso { get; set; }
        public Decimal SemIngresso { get; set; }
        public DateTime? DtColacao { get; set; }
        public DateTime? DtDiploma { get; set; }
        public DateTime? DtReabertura { get; set; }
        public Decimal AnoEncerramento { get; set; }
        public Decimal SemEncerramento { get; set; }
        public string CausaEncerr { get; set; }
        public string DiplomaFaculdade { get; set; }
        public string Registro { get; set; }
        public DateTime DtRegistro { get; set; }
        public string NomeDiploma { get; set; }
        public string Processo { get; set; }
        public string Livro { get; set; }
        public string Folhas { get; set; }
        public string NomeReitor { get; set; }
        public DateTime DtRetiraDip { get; set; }
        public Decimal NumOficio { get; set; }
        public DateTime DtOficio { get; set; }
        public string Observacao { get; set; }
        public string SitDiploma { get; set; }
        public DateTime DtInsercao { get; set; }
        public DateTime DtUltalt { get; set; }
        public string MotivoInvalidacao { get; set; }
        public DateTime DtConfeccao { get; set; }
        public DateTime DtAssinAluno { get; set; }
        public DateTime DtEnvioInstExt { get; set; }
        public string Motivoreabertura { get; set; }
        public decimal Pessoa { get; set; }

    }
}
