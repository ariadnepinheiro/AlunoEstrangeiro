using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class ExecucaoLogErro : IEntity
    {
        [AtributoCampo(Nome = "EXECUCAOLOGERROID")]
        public int ExecucaoLogErroId { get; private set; }

        [AtributoCampo(Nome = "EXECUCAOID")]
        public int ExecucaoId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "DESCRICAOERRO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }
    }
}
