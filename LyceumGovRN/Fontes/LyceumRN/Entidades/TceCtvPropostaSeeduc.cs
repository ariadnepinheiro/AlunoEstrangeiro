using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvPropostaSeeduc : IEntity
    {
        public int IdPropostaSeeduc { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public int VagasContinuidade { get; set; }

        public int VagasNovas { get; set; }

        public DateTime DtCadastro { get; set; }

        [AtributoCampo(Nome = "TAXAREPROVACAO")]
        public decimal TaxaReprovacao { get; set; }

        public string Matricula { get; set; }
    }
}
