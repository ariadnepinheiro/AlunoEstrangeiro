using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.MOTIVOREPROVACAOPLANILHAORCAMENTARIA", Nome = "PrestacaoContas.MOTIVOREPROVACAOPLANILHAORCAMENTARIA")]
    public class MotivoReprovacaoPlanilhaOrcamentaria : IEntity
    {
        [AtributoCampo(Nome = "MOTIVOREPROVACAOPLANILHAORCAMENTARIAID")]
        public int MotivoReprovacaoPlanilhaOrcamentariaId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
