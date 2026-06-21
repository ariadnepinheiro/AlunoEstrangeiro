using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.PERIODOLANCAMENTOFREQGLP", Nome = "RecursosHumanos.PERIODOLANCAMENTOFREQGLP")]
    public class PeriodoLancamentoFreqGLP : IEntity
    {
        [AtributoCampo(Nome = "PERIODOLANCAMENTOFREQGLPID")]
        public int PeriodoLancamentoFreqGLPId { get; set; }

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
