using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.ITEMPLANILHAORCAMENTARIA", Nome = "PrestacaoContas.ITEMPLANILHAORCAMENTARIA")]
    public class ItemPlanilhaOrcamentaria : IEntity
    {
        [AtributoCampo(Nome = "ITEMPLANILHAORCAMENTARIAID")]
        public int ItemPlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "PLANILHAORCAMENTARIAID")]
        public int PlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "FONTERECURSOID")]
        public int FonteRecursoId { get; set; }

        [AtributoCampo(Nome = "REFERENCIA")]
        public int Referencia { get; set; }

        [AtributoCampo(Nome = "VALOR")]
        public decimal Valor { get; set; }

        [AtributoCampo(Nome = "RETORNOREFERENCIA")]
        public string RetornoReferencia { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}