using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.PESSOAALUNO", Nome = "Matricula.PESSOAALUNO")]
    public class PessoaAluno : IEntity
    {
        [AtributoCampo(Nome = "PESSOAID")]
        public decimal PessoaId { get; set; }

        public string Aluno { get; set; }

        [AtributoCampo(Nome = "SITUACAOALUNO")]
        public string SituacaoAluno { get; set; }

        [AtributoCampo(Nome = "ANOENCERRAMENTO")]
        public int AnoEncerramento { get; set; }

        [AtributoCampo(Nome = "PERIODOENCERRAMENTO")]
        public int PeriodoEncerramento { get; set; }

        [AtributoCampo(Nome = "ESCOLAALUNO")]
        public string EscolaAluno { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
