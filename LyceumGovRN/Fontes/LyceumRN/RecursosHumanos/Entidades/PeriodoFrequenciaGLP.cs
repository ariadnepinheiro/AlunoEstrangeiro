using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.PERIODOFREQUENCIAGLP", Nome = "RecursosHumanos.PERIODOFREQUENCIAGLP")]
    public class PeriodoFrequenciaGlp : IEntity
    {
        [AtributoCampo(Nome = "PERIODOFREQUENCIAGLPID")]
        public int PeriodoFrequenciaGlpId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

    }
}
