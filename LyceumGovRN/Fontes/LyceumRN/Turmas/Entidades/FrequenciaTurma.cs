using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.FREQUENCIATURMA", Nome = "Turma.FREQUENCIATURMA")]
    public class FrequenciaTurma : IEntity
    {
        [AtributoCampo(Nome = "FREQUENCIATURMAID")]
        public int FrequenciaTurmaId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public int Periodo { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "MESREFERENCIA")]
        public int MesReferencia { get; set; }    

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

    }
}
