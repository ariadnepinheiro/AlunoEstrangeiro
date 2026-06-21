using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.ANALISEREPASSE", Nome = "PrestacaoContas.ANALISEREPASSE")]
    public class AnaliseRepasse : IEntity
    {
        [AtributoCampo(Nome = "ANALISEREPASSEID")]
        public int AnaliseRepasseId { get; set; }

        [AtributoCampo(Nome = "LANCAMENTOREPASSEID")]
        public int LancamentoRepasseId { get; set; }

        [AtributoCampo(Nome = "MOTIVOREPROVACAOLANCAMENTOREPASSEID")]
        public int? MotivoReprovacaoLancamentoRepasseId { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public bool Aprovado { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATAAPROVACAO")]
        public DateTime DataAprovacao { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

