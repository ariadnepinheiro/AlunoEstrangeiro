using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Reposicao.Entidades
{
    [AtributoTabela("Reposicao.PERIODOLANCAMENTO", Nome = "Reposicao.PERIODOLANCAMENTO")]
    public class PeriodoLancamento : IEntity
    {
        [AtributoCampo(Nome = "PERIODOLANCAMENTOID")]
        public int PeriodoLancamentoId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "DATAINICIOGREVE")]
        public DateTime DataInicioGreve { get; set; }

        [AtributoCampo(Nome = "DATAFIMGREVE")]
        public DateTime? DataFimGreve { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
