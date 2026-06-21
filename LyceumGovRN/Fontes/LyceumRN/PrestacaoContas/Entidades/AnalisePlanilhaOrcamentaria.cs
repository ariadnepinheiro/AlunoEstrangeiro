using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.ANALISEPLANILHAORCAMENTARIA", Nome = "PrestacaoContas.ANALISEPLANILHAORCAMENTARIA")]
    public class AnalisePlanilhaOrcamentaria : IEntity
    {
        [AtributoCampo(Nome = "ANALISEPLANILHAORCAMENTARIAID")]
        public int AnalisePlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "PLANILHAORCAMENTARIAID")]
        public int PlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "MOTIVOREPROVACAOPLANILHAORCAMENTARIAID")]
        public int? MotivoReprovacaoPlanilhaOrcamentariaId { get; set; }

        public int Ano { get; set; }

        [AtributoCampo(Nome = "APROVADA")]
        public bool Aprovada { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
