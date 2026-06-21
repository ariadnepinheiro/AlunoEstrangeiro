using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("LOGATUALIZACAOMATRICULA", Nome = "LOGATUALIZACAOMATRICULA")]
    public class LogAtualizacaoMatricula : IEntity
    {
        [AtributoCampo(Nome = "LOGATUALIZACAOMATRICULAID")]
        public int LogAtualizacaoMatriculaId { get; set; }

        [AtributoCampo(Nome = "DOCENTEID")]
        public int DocenteId { get; set; }

        [AtributoCampo(Nome = "IDFUNCIONALANTERIOR")]
        public int? IdFuncionalAnterior { get; set; }

        [AtributoCampo(Nome = "IDFUNCIONALNOVO")]
        public int? IdFuncionalNovo { get; set; }

        [AtributoCampo(Nome = "VINCULOANTERIOR")]
        public int? VinculoAnterior { get; set; }

        [AtributoCampo(Nome = "VINCULONOVO")]
        public int? VinculoNovo { get; set; }

        [AtributoCampo(Nome = "MATRICULAANTERIOR")]
        public string MatriculaAnterior { get; set; }

        [AtributoCampo(Nome = "MATRICULANOVA")]
        public string MatriculaNova { get; set; }

        [AtributoCampo(Nome = "USUARIO")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }
    }
}
