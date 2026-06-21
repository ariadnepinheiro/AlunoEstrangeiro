using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RenovacaoMatricula.Entidades
{
    [AtributoTabela("RENOVACAO_CONFIRMACAOMATRICULA", Nome = "RENOVACAO_CONFIRMACAOMATRICULA")]
    public class RenovacaoConfirmacaoMatricula : IEntity
    {
        [AtributoCampo(Nome = "RENOVACAOID")]
        public int RenovacaoId { get; set; }

        [AtributoCampo(Nome = "CONFIRMACAOMATRICULAID")]
        public DateTime ConfirmacaoMatriculaId { get; set; }
    }
}
