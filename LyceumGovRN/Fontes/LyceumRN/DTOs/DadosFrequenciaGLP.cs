using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosFrequenciaGLP
    {
        public int ANO_FILTRADO { get; set; }
        public int MES_FILTRADO { get; set; }
        public decimal NUM_FUNC { get; set; }
        public string MES_EXTENSO { get; set; }
        public int ID_REGIONAL { get; set; }
        public string NOME_REGIONAL { get; set; }
        public string ID_MUNICIPIO { get; set; }
        public string NOME_MUNICIPIO { get; set; }
        public string UNIDADE_ENS { get; set; }
        public string NOME_UNIDADE_ENS { get; set; }
        public string SETOR_UNIDADE_ENS { get; set; }
        public int? IDFUNCIONAL { get; set; }
        public int? VINCULO { get; set; }
        public string MATRICULA { get; set; }
        public string NOME { get; set; }
        public DateTime DATA_INICIO { get; set; }
        public DateTime DATA_FIM { get; set; }
        public int? CH_SEMANAL { get; set; }
        public int CH_MENSAL { get; set; }
        public int? ID_CARGAHNAOTRABMES { get; set; }
        public int? ANO { get; set; }
        public int? MES { get; set; }
        public int CHNAOTRABALHADAMES { get; set; }
        public int CH_MENSAL_TOTAL { get; set; }
        public string IDVINCULO { get; set; }
        public string PERIODO { get; set; }
    }
}
