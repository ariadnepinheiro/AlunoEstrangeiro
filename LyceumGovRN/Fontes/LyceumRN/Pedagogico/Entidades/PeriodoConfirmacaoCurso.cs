using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    [AtributoTabela("PrestacaoContas.PERIODOCONFIRMACAOCURSO", Nome = "PrestacaoContas.PERIODOCONFIRMACAOCURSO")]
    public class PeriodoConfirmacaoCurso : IEntity
    {
        [AtributoCampo(Nome = "PERIODOCONFIRMACAOCURSOID")]
        public int PeriodoConfirmacaoCursoId { get; set; }

        [AtributoCampo(Nome = "PERIODOCONFIRMACAOID")]
        public int PeriodoConfirmacaoId { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

