using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Reposicao.Entidades
{
    [AtributoTabela("Reposicao.REPOSICAO", Nome = "Reposicao.REPOSICAO")]
    public class Reposicao : IEntity
    {
        [AtributoCampo(Nome = "REPOSICAOID")]
        public int ReposicaoId { get; set; }

        [AtributoCampo(Nome = "NUM_FUNC")]
        public int NumFunc { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "DISCIPLINA")]
        public string Disciplina { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "SEMESTRE")]
        public int Semestre { get; set; }

        [AtributoCampo(Nome = "TIPO_AULA")]
        public string TipoAula { get; set; }

        [AtributoCampo(Nome = "DATAREPOSICAO")]
        public DateTime DataReposicao { get; set; }

        [AtributoCampo(Nome = "CHREPOSICAO")]
        public int CHReposicao { get; set; }

        [AtributoCampo(Nome = "DATAGREVE")]
        public DateTime DataGreve { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
