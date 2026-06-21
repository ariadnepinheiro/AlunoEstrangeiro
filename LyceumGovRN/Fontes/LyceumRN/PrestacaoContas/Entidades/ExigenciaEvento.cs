using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.EXIGENCIAEVENTO", Nome = "PrestacaoContas.EXIGENCIAEVENTO")]
    public class ExigenciaEvento : IEntity
    {
        [AtributoCampo(Nome = "EXIGENCIAEVENTOID")]
        public int ExigenciaEventoId { get; set; }

        [AtributoCampo(Nome = "MOTIVOEXIGENCIAEVENTOID")]
        public int MotivoExigenciaEventoId { get; set; }

        [AtributoCampo(Nome = "EVENTOID")]
        public int EventoId { get; set; }

        [AtributoCampo(Nome = "NOTAEXPLICATIVA")]
        public string NotaExplicativa { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public bool Aprovado { get; set; }

        [AtributoCampo(Nome = "REJEITADO")]
        public bool Rejeitado { get; set; }

        [AtributoCampo(Nome = "CUMPRIDA")]
        public bool Cumprida { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
