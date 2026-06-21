using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PLANILHAORCAMENTARIA", Nome = "PrestacaoContas.PLANILHAORCAMENTARIA")]
    public class PlanilhaOrcamentaria : IEntity
    {
        [AtributoCampo(Nome = "PLANILHAORCAMENTARIAID")]
        public int PlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "NATUREZADESPESAID")]
        public int NaturezaDespesaId { get; set; }

        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "PROGRAMATRABALHOID")]
        public int ProgramaTrabalhoId { get; set; }

        [AtributoCampo(Nome = "REGIAOFINANCEIRAID")]
        public int RegiaoFinanceiraId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "PROCESSO")]
        public string Processo { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
