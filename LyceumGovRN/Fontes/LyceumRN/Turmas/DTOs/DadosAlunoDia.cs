using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Turmas.DTOs
{
   public class DadosAlunoDia
   {
       public string Aluno { get; set; }

       public DateTime Data { get; set; }

       public bool FaltaDia { get; set; }

       public DateTime? UltimoDiaLancamento { get; set; }
    }
}
