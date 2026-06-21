using System;
using System.Collections.Generic;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
    public class DTOListaDISPONIBILIDADEGLP
    {
        public int DISPONIBILIDADEGLPID { get; set; }
        public string UNIDADE_ENS { get; set; }
        public decimal NUM_FUNC { get; set; }
        public string AGRUPAMENTO { get; set; }
        public int ANO { get; set; }
        public string REGIONAL { get; set; }
        public string MUNICIPIO { get; set; }
        public string UNIDADE_ESCOLAR { get; set; }
        public string DISCIPLINA { get; set; }
        public string USUARIOID { get; set; }
        public DateTime? DATACADASTRO { get; set; }
        public DateTime? DATAALTERACAO { get; set; }
        public string MODALIDADE { get; set; }
        public string DIASEMANA { get; set; }
        public string TURNO { get; set; }
    }
}
