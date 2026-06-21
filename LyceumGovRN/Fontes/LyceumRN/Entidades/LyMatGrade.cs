using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyMatGrade : IEntity
    {
        public string Aluno { get; set; }

        public decimal GradeId { get; set; }

        public decimal NumChamada { get; set; }

        public string SitMatgrade { get; set; }

        public DateTime DtUltalt { get; set; }
    }
}
