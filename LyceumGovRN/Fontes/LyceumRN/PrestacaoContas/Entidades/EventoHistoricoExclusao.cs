using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.EVENTOHISTORICOEXCLUSAO", Nome = "PrestacaoContas.EVENTOHISTORICOEXCLUSAO")]
    public class EventoHistoricoExclusao : IEntity
    {
        [AtributoCampo(Nome = "EVENTOHISTORICOEXCLUSAOID")]
        public int EventoHistoricoExclusaoId { get; set; }

        [AtributoCampo(Nome = "EVENTOID")]
        public int EventoId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "FORNECEDORID")]
        public int FornecedorId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVAORCAMENTO")]
        public string JustificativaOrcamento { get; set; }

        [AtributoCampo(Nome = "CHAVEACESSO")]
        public string ChaveAcesso { get; set; }

        [AtributoCampo(Nome = "NUMERONOTAFISCAL")]
        public string NumeroNotaFiscal { get; set; }

        [AtributoCampo(Nome = "VALORNOTAFISCAL")]
        public decimal ValorNotaFiscal { get; set; }

        [AtributoCampo(Nome = "DATANOTAFISCAL")]
        public DateTime DataNotaFiscal { get; set; }

        [AtributoCampo(Nome = "OBSERVACOES")]
        public string Observacoes { get; set; }

        [AtributoCampo(Nome = "EVIDENCIAS")]
        public string Evidencias { get; set; }

        [AtributoCampo(Nome = "DATAPAGAMENTO")]
        public DateTime DataPagamento { get; set; }

        [AtributoCampo(Nome = "VALORPAGAMENTO")]
        public decimal ValorPagamento { get; set; }

        [AtributoCampo(Nome = "NUMEROEVENTO")]
        public string NumeroEvento { get; set; }

        [AtributoCampo(Nome = "TIPODESPESA")]
        public int TipoDespesa { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public bool? Aprovado { get; set; }

        [AtributoCampo(Nome = "DATAAPROVACAO")]
        public DateTime DataAprovacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "USUARIOAPROVACAO")]
        public string UsuarioAprovacao { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEEXIGENCIAS")]
        public DateTime QuantidadeExigencias { get; set; }

        [AtributoCampo(Nome = "MOTIVOEXCLUSAO")]
        public DateTime MotivoExclusao { get; set; }

        [AtributoCampo(Nome = "USUARIOEXCLUSAO")]
        public string UsuarioExclusao { get; set; }

        [AtributoCampo(Nome = "DATAEXCLUSAO")]
        public DateTime DataExclusao { get; set; }
    }
}

