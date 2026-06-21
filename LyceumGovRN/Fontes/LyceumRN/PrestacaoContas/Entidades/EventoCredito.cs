using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.EVENTOCREDITO", Nome = "PrestacaoContas.EVENTOCREDITO")]
    public class EventoCredito : IEntity
    {
        [AtributoCampo(Nome = "EVENTOCREDITOID")]
        public int EventoCreditoId { get; set; }

        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "EXIGENCIAEVENTOID")]
        public int? ExigenciaEventoId { get; set; } // quando vem de uma exigencia de evento

        [AtributoCampo(Nome = "EXIGENCIAEXTRATOID")]
        public int? ExigenciaExtratoId { get; set; } //quando vem de uma exigencia de extrato

        [AtributoCampo(Nome = "VALOR")]
        public decimal Valor { get; set; }

        [AtributoCampo(Nome = "DATAEVENTO")]
        public DateTime DataEvento { get; set; }

        [AtributoCampo(Nome = "NUMEROEVENTO")]
        public string NumeroEvento { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

