using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosTurmaGLP
    {
        public int ANO { get; set; }
        public int MES { get; set; }
        public string FACULDADE { get; set; }
        public decimal NUM_FUNC { get; set; }
        public string MES_EXTENSO { get; set; }
        public string TURMA { get; set; }
        public string NOME_DISCIPLINA { get; set; }
        public string DISCIPLINA { get; set; }
        public DateTime DATA_INICIO { get; set; }
        public DateTime DATA_FIM { get; set; }
        public int? CH_SEMANAL { get; set; }
        public int CH_MENSAL { get; set; }
    }
}
