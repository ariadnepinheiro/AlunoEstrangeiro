using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.ContratoTemporario.Entidades
{
    [AtributoTabela("ContratoTemporario.CANDIDATODOCENTE_GRUPOHABILITACAO", Nome = "ContratoTemporario.CANDIDATODOCENTE_GRUPOHABILITACAO")]
    public class CandidatoDocente_GrupoHabilitacao
    {
        public string Concurso { get; set; }

        public string Candidato { get; set; }

        public string Agrupamento { get; set; }

        public bool Habilitado { get; set; }

        public string NomeDisciplina { get; set; }
    }
}
