using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceEquipamentoUnidadeFisica
    {
        public int IdEquipamentoUnidadeFisica { get; set; }

        public string UnidadeFisica { get; set; }

        public int IdEquipamento { get; set; }

        public int Quantidade { get; set; }

        public string Matricula { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
