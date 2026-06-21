using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PeDeMeiaCorrecao.Entidades
{
    [AtributoTabela("PeDeMeiaCorrecao.FREQUENCIAALUNO", Nome = "PeDeMeiaCorrecao.FREQUENCIAALUNO")]
    public class FrequenciaAluno : IEntity
    {
        [AtributoCampo(Nome = "FREQUENCIAALUNOID")]
        public int FrequenciaAlunoId { get; set; }

        [AtributoCampo(Nome = "FREQUENCIATURMAID")]
        public int FrequenciaTurmaId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "DATAAUSENCIA")]
        public DateTime DataAusencia { get; set; }      

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

    }
}
