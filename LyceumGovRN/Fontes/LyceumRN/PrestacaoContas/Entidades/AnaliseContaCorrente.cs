using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.ANALISECONTACORRENTE", Nome = "PrestacaoContas.ANALISECONTACORRENTE")]
    public class AnaliseContaCorrente : IEntity
    {
        [AtributoCampo(Nome = "ANALISECONTACORRENTEID")]
        public int AnaliseContaCorrenteId { get; set; }

        [AtributoCampo(Nome = "CONTACORRENTEID")]
        public int ContaCorrenteId { get; set; }

        [AtributoCampo(Nome = "MOTIVOREPROVACAOCONTACORRENTEID")]
        public int? MotivoReprovacaoContaCorrenteId { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public bool Aprovado { get; set; }

        [AtributoCampo(Nome = "USUARIOAPROVACAOID")]
        public string UsuarioAprovacaoId { get; set; }

        [AtributoCampo(Nome = "DATAAPROVACAO")]
        public DateTime DataAprovacao { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
