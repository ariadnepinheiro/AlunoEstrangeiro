using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.EXTRATOBANCARIO", Nome = "PrestacaoContas.EXTRATOBANCARIO")]
    public class ExtratoBancario : IEntity
    {
        [AtributoCampo(Nome = "EXTRATOBANCARIOID")]
        public int ExtratoBancarioId { get; set; }

        [AtributoCampo(Nome = "PERIODOREFERENCIAEXTRATOBANCARIOID")]
        public int PeriodoReferenciaExtratoBancarioId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "MES")]
        public int Mes { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "OBSERVACAO")]
        public string Observacao { get; set; }

        [AtributoCampo(Nome = "APROVADO")]
        public bool? Aprovado { get; set; } 

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATAAPROVACAO")]
        public DateTime DataAprovacao { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

