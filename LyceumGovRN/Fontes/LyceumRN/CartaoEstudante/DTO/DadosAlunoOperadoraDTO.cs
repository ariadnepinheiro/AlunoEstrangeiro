using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class DadosAlunoOperadoraDTO
    {
        public string LoginOperadora { get; set; }

        public string NomeOperadora{ get; set; }
        
        public int AlunoOperadoraId  { get; set; }

        public DateTime DataAtualizacaoLogin { get; set; }
    }
}
