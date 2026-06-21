using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Agenda.Entidades
{
    [AtributoTabela("Matricula.PARAMETROBLOQUEIOENCERRAMENTO", Nome = "Matricula.PARAMETROBLOQUEIOENCERRAMENTO")]
    public class ParametroBloqueioEncerramento : IEntity
    {
        [AtributoCampo(Nome = "PARAMETROBLOQUEIOENCERRAMENTOID")]
        public int ParametroBloqueioEncerramentoId { get; set; }

        [AtributoCampo(Nome = "AGENDAID")]
        public int AgendaId { get; set; }

        [AtributoCampo(Nome = "BLOQUEIOTOTAL")]
        public bool BloqueioTotal { get; set; }

        [AtributoCampo(Nome = "VALIDARENOVACAO")]
        public bool ValidaRenovacao { get; set; }

        [AtributoCampo(Nome = "VALIDAMATRICULAFACIL")]
        public bool ValidaMatriculaFacil { get; set; }
    }
}
