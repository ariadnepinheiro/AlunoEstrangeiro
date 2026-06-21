using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RenovacaoMatricula.Entidades
{
    [AtributoTabela("UNIDADEENSINORENOVACAOAUTOMATICA", Nome = "UNIDADEENSINORENOVACAOAUTOMATICA")]
    public class UnidadeEnsinoRenovacaoAutomatica : IEntity
    {
        [AtributoCampo(Nome = "UNIDADEENSINIORENOVACAOAUTOMATICAID")]
        public int UnidadeEnsinioRenovacaoAutomaticaId { get; set; }

        [AtributoCampo(Nome = "UNIDADEENSINOID")]
        public string UnidadeEnsinoId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "USUARIO")]
        public string Usuario { get; set; }
    }
}
