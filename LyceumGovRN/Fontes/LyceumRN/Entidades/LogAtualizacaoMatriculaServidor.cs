using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("LOGATUALIZACAOMATRICULASERVIDOR", Nome = "LOGATUALIZACAOMATRICULASERVIDOR")]
    public class LogAtualizacaoMatriculaServidor : IEntity
    {
        [AtributoCampo(Nome = "LOGATUALIZACAOMATRICULASERVIDORID")]
        public int LogAtualizacaoMatriculaServidorId { get; set; }

        [AtributoCampo(Nome = "PESSOA")]
        public int Pessoa { get; set; }

        [AtributoCampo(Nome = "ORDEM")]
        public int Ordem { get; set; }

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
