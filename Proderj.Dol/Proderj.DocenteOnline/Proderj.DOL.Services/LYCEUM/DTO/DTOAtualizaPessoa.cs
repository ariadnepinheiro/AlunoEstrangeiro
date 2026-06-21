using System;
using System.Collections.Generic;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
    public class DTOAtualizaPessoa
    {
        public double PESSOA { get; set; }
        public string END_PAIS { get; set; }
        public string CEP { get; set; }
        public string ENDERECO { get; set; }
        public string END_NUM { get; set; }
        public string END_COMPL { get; set; }
        public string BAIRRO { get; set; }
        public string END_MUNICIPIO { get; set; }
        public char? AREA_QUILOMBOS { get; set; }
        public char? TERRA_INDIGENA { get; set; }
        public char? AREA_ASSENTAMENTO { get; set; }
        public string FONE { get; set; }
        public string CELULAR { get; set; }
        public string E_MAIL_INTERNO { get; set; }
        public string E_MAIL { get; set; }
        public string FL_FIELD_01 { get; set; }
        public string USUARIOID { get; set; }
        public DateTime? DATAALTERACAO { get; set; }
    }
}
