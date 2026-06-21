using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DadosCapacitacaoDocenteDTO
    {
        public string OferecidoSEEDUC { get; set; }
        public string Capacitacao { get; set; }
        public string TipoCurso { get; set; }
        public string AreaConhecimento { get; set; }
        public string NomeInstituicao { get; set; }
        public int CargaHoraria { get; set; }
        public DateTime DataConclusao { get; set; }
    }
}
