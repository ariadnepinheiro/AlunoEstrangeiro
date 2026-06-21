using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("LY_DOCENTE_FUNCAO_GLP_DETALHE", Nome = "LY_DOCENTE_FUNCAO_GLP_DETALHE")]
    public class LyDocenteFuncaoGlpDetalhe
    {
        [AtributoCampo(Nome = "ID_DOCENTE_FUNCAO_GLP")]
        public decimal IdDocenteFuncaoGlp { get; set; }

        public DateTime Data { get; set; }

        public string Status { get; set; }

        [AtributoCampo(Nome = "QTD_GLP")]
        public decimal? QtdGlp { get; set; }

        public string Usuario { get; set; }

        public string Motivo { get; set; }
    }
}
