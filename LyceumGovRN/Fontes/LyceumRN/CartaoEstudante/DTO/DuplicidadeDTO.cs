using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class DuplicidadeDTO
    {
        public int DUPLICIDADEID { get; set; }
        public int OPERADORAID { get; set; }
        public int IDBENEFICIARIO { get; set; }
        public string ALUNO { get; set; }
        public string NOMEOPERADORA { get; set; }
        public string NOME_COMPL { get; set; }
        public string SIT_ALUNO { get; set; }
        public string FLAGMATRICULAPRINCIPAL { get; set; }
        public DateTime DATAINCLUSAO { get; set; }
        public DateTime DATAATUALIZACAO { get; set; }
        public long NumeroRegistro { get; set; }
    }
}
