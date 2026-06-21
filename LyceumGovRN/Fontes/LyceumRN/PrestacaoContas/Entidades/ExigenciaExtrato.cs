using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.EXIGENCIAEXTRATO", Nome = "PrestacaoContas.EXIGENCIAEXTRATO")]
    public class ExigenciaExtrato : IEntity
    {
        [AtributoCampo(Nome = "EXIGENCIAEXTRATOID")]
        public int ExigenciaExtratoId { get; set; }

        [AtributoCampo(Nome = "TIPOEXIGENCIAEXTRATOID")]
        public int TipoExigenciaExtratoId { get; set; }

        [AtributoCampo(Nome = "EXTRATOBANCARIOID")]
        public int ExtratoBancarioId { get; set; }
       
        [AtributoCampo(Nome = "NOTAEXPLICATIVA")]
        public string NotaExplicativa { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public bool? Aprovado { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
