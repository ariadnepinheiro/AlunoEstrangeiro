using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosRegional
    {
        public int ID_REGIONAL { get; set; }
        public string REGIONAL { get; set; }
        public string SETOR { get; set; }
        public string CEP { get; set; }
        public string MUNICIPIO { get; set; }
        public string LOGRADOURO { get; set; }
        public string NUMERO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string MATRICULA { get; set; }
        public DateTime DT_CADASTRO { get; set; }
        public DateTime DT_ALTERACAO { get; set; }
        public string CNPJ { get; set; }
    }
}
