using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
  [AtributoTabela("PrestacaoContas.OPERACAOEXIGENCIA", Nome = "PrestacaoContas.OPERACAOEXIGENCIA")]
    public class OperacaoExigencia : IEntity
    {
        [AtributoCampo(Nome = "OPERACAOEXIGENCIAID")]
        public int OperacaoExigenciaId { get; set; }

        [AtributoCampo(Nome = "OPERACAOID")]
        public int OperacaoId { get; set; }

        [AtributoCampo(Nome = "TIPOEXIGENCIAOPERACAOID")]
        public int TipoExigenciaOperacaoId { get; set; }

        [AtributoCampo(Nome = "NOTAEXPLICATIVA")]
        public string NotaExplicativa { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public int Aprovado { get; set; }
        
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
