using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyCapacitacao : IEntity
    {
        public int Pessoa { get; set; }
        public int Ordem { get; set; }
        public string Capacitacao { get; set; }
        public DateTime DataConclusao { get; set; }
        public string NomeInstituicao { get; set; }
        public int CargaHoraria { get; set; }
        public int TipoCursoCapacitacaoId { get; set;}
        public int AreaConhecimentoId { get; set; }
    }
}
