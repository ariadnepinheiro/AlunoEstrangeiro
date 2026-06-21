using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades
{
    public class Inscricao
    {
        public int InscricaoId { get; set; }
        public int CandidatoId { get; set; }
        public int ProcessoSeletivoId { get; set; }
        public Int64 NumeroInscricao { get; set; }
        public RN.ProcessoSeletivoAluno.Inscricao.Situacao Situacao { get; set; }
        public string IP { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
