using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("dbo.LY_ALUNO_LICENCA", Nome = "dbo.LY_ALUNO_LICENCA")]
    public class LyAlunoLicenca : IEntity
    {
        [AtributoCampo(Nome = "ID_ALUNO_LICENCA")]
        public int IdAlunoLicenca { get; set; }

        public string Aluno { get; set; }

        public decimal Ano { get; set; }

        public decimal Semestre { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }

        //public string Descricao { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVAFALTAID")]
        public int JustificativaFaltaId { get; set; }

        public string Observacao { get; set; }

        [AtributoCampo(Nome = "DT_INICIO")]
        public DateTime DtInicio { get; set; }

        [AtributoCampo(Nome = "DT_FIM")]
        public DateTime DtFim { get; set; }

        public string Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "STAMP_ATUALIZACAO")]
        public DateTime StampAtualizacao { get; set; }
    }
}
