using System;
using System.Collections.Generic;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
    public class DTOIncluiDISPONIBILIDADEGLP
    {
        /*
        public int CodigoRegional { get; set; }
        public string CodigoMunicipio { get; set; }
        public string CodigoUE { get; set; }
        public string CodigoDisciplina { get; set; }
        public int[] CodigoModalidade { get; set; }
        public int[] CodigoDiaSemana { get; set; }
        public int[] CodigoTurno { get; set; } 
        */

        public string AGRUPAMENTO { get; set; }
        public int? NUM_FUNC { get; set; }
        public int ANO { get; set; }
        public string USUARIOID { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public IList<string> UNIDADE_ENS { get; set; }
        public IList<string> MODALIDADE { get; set; }
        public IList<DiaDaSemanaEnum> DIASEMANA { get; set; }
        public IList<string> TURNO { get; set; }
    }
}
