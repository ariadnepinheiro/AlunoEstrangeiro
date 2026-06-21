using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO", Nome = "PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO")]
    public class PeriodoReferenciaExtratoBancario : IEntity
    {
        [AtributoCampo(Nome = "PERIODOREFERENCIAEXTRATOBANCARIOID")]
        public int PeriodoReferenciaExtratoBancarioId { get; set; }

        [AtributoCampo(Nome = "DIAREFERENCIA")]
        public int DiaReferencia { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
