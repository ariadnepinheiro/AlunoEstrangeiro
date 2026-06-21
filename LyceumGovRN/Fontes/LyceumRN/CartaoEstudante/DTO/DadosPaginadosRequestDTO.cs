using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class DadosPaginadosRequestDTO
    {
        public DateTime DtAtualizacaoInicial { get; set; }
        public DateTime DtAtualizacaoFinal { get; set; }
        public int RegInicial { get; set; }
        public int RegFinal { get; set; }

        public override string ToString()
        {
            return string.Format(
                "DtAtualizacaoInicial: {0}, DtAtualizacaoFinal: {1}, RegInicial: {2}, RegFinal: {3}", 
                DtAtualizacaoInicial.ToString("dd/MM/yyyy"), 
                DtAtualizacaoFinal.ToString("dd/MM/yyyy"),
                RegInicial, 
                RegFinal
            );
        }
    }
}
