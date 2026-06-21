using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.FREQUENCIAPLANODEAULA", Nome = "Turma.FREQUENCIAPLANODEAULA")]
    public class FrequenciaPlanoDeAula : IEntity
    {
        [AtributoCampo(Nome = "FREQUENCIAPLANODEAULAID")]
        public int FrequenciaPlanoDeAulaId { get; set; }

        public decimal Ano { get; set; }

        public decimal Semestre { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }

        [AtributoCampo(Nome = "DATAFREQUENCIA")]
        public DateTime DataFrequencia { get; set; }

        [AtributoCampo(Nome = "PLANOAULA")]
        public string PlanoAula { get; set; }

        [AtributoCampo(Nome = "NUMFUNCLANCAMENTO")]
        public decimal? NumFuncLancamento { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [AtributoCampo(Nome = "NOMEUSUARIO")]
        public string NomeUsuario { get; set; }
    }
}
