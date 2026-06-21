using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosEquipamentoUnidadeFisica
    {
        public int IdEquipamentoUnidadeFisica { get; set; }

        public string UnidadeFisica { get; set; }

        public string Descricao { get; set; }

        public int IdEquipamento { get; set; }

        public int Quantidade { get; set; }

        public string Matricula { get; set; }

        public DateTime DtAlteracao { get; set; }

        public int QuantidadeMaximaSugerida { get; set; }

        public int? IdEquipamentoMaximoVinculado { get; set; }
    }
}
