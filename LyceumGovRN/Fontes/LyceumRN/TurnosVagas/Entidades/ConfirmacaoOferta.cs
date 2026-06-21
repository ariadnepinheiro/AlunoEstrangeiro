using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    [AtributoTabela("TurnosVagas.CONFIRMACAOOFERTA", Nome = "TurnosVagas.CONFIRMACAOOFERTA")]
    public class ConfirmacaoOferta : IEntity
    {
        [AtributoCampo(Nome = "CONFIRMACAOOFERTAID")]
        public int ConfirmacaoOfertaId { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public int Periodo { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "CURSOREFERENCIA")]
        public string CursoReferencia { get; set; }

        [AtributoCampo(Nome = "SERIEREFERENCIA")]
        public int SerieReferencia { get; set; }

        [AtributoCampo(Nome = "MATRICULADOSMANHA")]
        public int MatriculadosManha { get; set; }

        [AtributoCampo(Nome = "MATRICULADOSTARDE")]
        public int MatriculadosTarde { get; set; }

        [AtributoCampo(Nome = "MATRICULADOSNOITE")]
        public int MatriculadosNoite { get; set; }

        [AtributoCampo(Nome = "MATRICULADOSINTEGRAL")]
        public int MatriculadosIntegral { get; set; }

        [AtributoCampo(Nome = "FINALIZADO")]
        public bool Finalizado { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
