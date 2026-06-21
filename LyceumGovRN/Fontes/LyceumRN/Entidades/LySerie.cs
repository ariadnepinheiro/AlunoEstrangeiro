using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
   public class LySerie : IEntity
    {
        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Curriculo { get; set; }

        public decimal Serie { get; set; }

        public string Descricao { get; set; }

        public string Complemento1 { get; set; }

        public string Complemento2 { get; set; }

        public decimal? IdadeMinima { get; set; }

        public decimal? DiaAniv { get; set; }

        public decimal? MesAniv { get; set; }

        public DateTime? DtExtincao { get; set; }

        public string CursoSeguinte { get; set; }

        public decimal? SerieSeguinte { get; set; }

        public string AnoSerieConcluinte { get; set; }

        public string EmiteCertificacao { get; set; } 

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? StampAtualizacao { get; set; }

        public string OfertaEletiva { get; set; } 
    }
}
