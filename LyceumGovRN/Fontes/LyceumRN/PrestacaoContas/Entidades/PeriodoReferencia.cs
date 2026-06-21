
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PERIODOREFERENCIA", Nome = "PrestacaoContas.PERIODOREFERENCIA")]
    public class PeriodoReferencia : IEntity
    {
        [AtributoCampo(Nome = "PERIODOREFERENCIAID")]
        public int PeriodoReferenciaId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "MESINICIAL")]
        public int MesInicial { get; set; }

        [AtributoCampo(Nome = "MESFINAL")]
        public int MesFinal { get; set; }

        [AtributoCampo(Nome = "REFERENCIA")]
        public string Referencia { get; set; }

        [AtributoCampo(Nome = "ANOANTERIOR")]
        public int AnoAnterior { get; set; }

        [AtributoCampo(Nome = "MESANTERIOR")]
        public int MesAnterior { get; set; }

        [AtributoCampo(Nome = "DATALIMITEPRESTACAOCONTAS")]
        public DateTime DataLimitePrestacaoContas { get; set; }

        [AtributoCampo(Nome = "DATALIMITEANALISE")]
        public DateTime DataLimiteAnalise { get; set; }

        [AtributoCampo(Nome = "DATALIMITEDESPESAS")]
        public DateTime DataLimiteDespesas { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
